using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using ST.Entities.Extensions;
using ST.Entities.Models.Pages;
using ST.Entities.Services.Abstraction;

namespace ST.CORE.Installation
{
	public static class MenuSync
	{
		/// <summary>
		/// Navbar id
		/// </summary>
		public static Guid NavBarId => Guid.Parse("46EACBA3-D515-47B0-9BA7-5391CE1D26B1".ToLower());

		/// <summary>
		/// List of menus
		/// </summary>
		public static List<Menu> Menu = new List<Menu>
		{
			new Menu
			{
				Id = NavBarId,
				Name = "Main Navbar",
				Description = "Default navbar for website",
				Author = "System",
				Created = DateTime.Now
			}
		};

		/// <summary>
		/// Sync default menus
		/// </summary>
		/// <param name="service"></param>
		public static async Task SyncMenuItems(IDynamicEntityDataService service)
		{
			var exists = await service.Any<Menu>();
			if (exists) return;
			var rq = await service.AddRange(Menu);
			if (rq.All(x => x.IsSuccess))
			{
				foreach (var item in GetMenus())
				{
					item.Created = DateTime.Now;
					item.Changed = DateTime.Now;
					var res = await service.AddSystem(item.Adapt<MenuItem>());
					if (!res.IsSuccess) continue;
					foreach (var i in item.SubItems)
					{
						var obj = i.Adapt<MenuItem>();
						obj.ParentMenuItemId = res.Result;
						obj.Created = DateTime.Now;
						obj.Changed = DateTime.Now;
						var r = await service.AddSystem(obj);
						if (!r.IsSuccess || i.SubItems == null) continue;
						foreach (var j in i.SubItems)
						{
							var ob = j.Adapt<MenuItem>();
							ob.ParentMenuItemId = r.Result;
							ob.Created = DateTime.Now;
							ob.Changed = DateTime.Now;
							var r1 = await service.AddSystem(ob);
							if (!r1.IsSuccess || j.SubItems == null) continue;
							foreach (var m in j.SubItems)
							{
								var ob1 = m.Adapt<MenuItem>();
								ob1.ParentMenuItemId = r1.Result;
								ob1.Created = DateTime.Now;
								ob1.Changed = DateTime.Now;
								await service.AddSystem(ob1);
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// Read menus
		/// </summary>
		/// <returns></returns>
		private static IEnumerable<MenuViewModel> GetMenus()
			=> JsonParser.ReadArrayDataFromJsonFile<List<MenuViewModel>>(Path.Combine(AppContext.BaseDirectory, "menus.json"));
	}
}