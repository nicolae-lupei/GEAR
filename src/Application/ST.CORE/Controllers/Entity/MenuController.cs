using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ST.BaseBusinessRepository;
using ST.CORE.Attributes;
using ST.CORE.Models;
using ST.Entities.Data;
using ST.Entities.Models.Pages;
using ST.Entities.Services.Abstraction;

namespace ST.CORE.Controllers.Entity
{
	public class MenuController : Controller
	{
		/// <summary>
		/// Context
		/// </summary>
		private readonly EntitiesDbContext _context;

		/// <summary>
		/// Inject Data Service
		/// </summary>
		private readonly IDynamicEntityDataService _dataService;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="context"></param>
		/// <param name="dataService"></param>
		public MenuController(EntitiesDbContext context, IDynamicEntityDataService dataService)
		{
			_context = context;
			_dataService = dataService;
		}

		/// <summary>
		/// Index view
		/// </summary>
		/// <returns></returns>
		public IActionResult Index()
		{
			return View();
		}

		/// <summary>
		/// Create view
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}

		/// <summary>
		/// Create new page type
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		public async Task<IActionResult> Create(Menu model)
		{
			if (model != null)
			{
				var req = await _dataService.AddSystem(model);
				if (req.IsSuccess)
					return RedirectToAction("Index");
				ModelState.AddModelError(string.Empty, "Fail to save!");
			}

			return View(model);
		}

		/// <summary>
		/// Edit page type
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet]
		public async Task<IActionResult> Edit(Guid id)
		{
			if (id.Equals(Guid.Empty)) return NotFound();
			var model = await _dataService.GetByIdSystem<Menu, Menu>(id);

			if (!model.IsSuccess) return NotFound();

			return View(model.Result);
		}

		/// <summary>
		/// Edit page type
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		public async Task<IActionResult> Edit(Menu model)
		{
			if (model == null) return NotFound();
			var dataModel = (await _dataService.GetByIdSystem<Menu, Menu>(model.Id)).Result;

			if (dataModel == null) return NotFound();

			dataModel.Name = model.Name;
			dataModel.Description = model.Description;
			dataModel.Author = model.Author;
			dataModel.Changed = DateTime.Now;
			var req = await _dataService.UpdateSystem(dataModel);
			if (req.IsSuccess) return RedirectToAction("Index");
			ModelState.AddModelError(string.Empty, "Fail to save");
			return View(model);
		}

		/// <summary>
		/// Get menu items
		/// </summary>
		/// <param name="menuId"></param>
		/// <param name="parentId"></param>
		/// <returns></returns>
		[HttpGet]
		public async Task<ActionResult> GetMenu(Guid menuId, Guid? parentId = null)
		{
			ViewBag.MenuId = menuId;
			ViewBag.ParentId = parentId;
			ViewBag.Menu = (await _dataService.GetByIdSystem<Menu, Menu>(menuId)).Result;
			ViewBag.Parent = (parentId != null) ?
									(await _dataService.GetByIdSystem<MenuItem, MenuItem>(parentId.Value)).Result
									: null;
			return View();
		}

		/// <summary>
		/// Create menu item
		/// </summary>
		/// <param name="menuId"></param>
		/// <param name="parentId"></param>
		/// <returns></returns>
		[HttpGet]
		public ActionResult CreateItem(Guid menuId, Guid? parentId = null)
		{
			ViewBag.MenuId = menuId;
			ViewBag.ParentId = parentId;
			ViewBag.Routes = _context.Pages.Where(x => !x.IsDeleted && !x.IsLayout).Select(x => x.Path);
			return View();
		}

		/// <summary>
		/// Create menu item
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		public async Task<IActionResult> CreateItem(MenuItem model)
		{
			ViewBag.Routes = _context.Pages.Where(x => !x.IsDeleted && !x.IsLayout).Select(x => x.Path);
			if (model != null)
			{
				model.AllowedRoles = "Administrator#";
				var req = await _dataService.AddSystem(model);
				if (req.IsSuccess)
					return RedirectToAction("GetMenu", new
					{
						model.MenuId,
						ParentId = model.ParentMenuItemId
					});
				ModelState.AddModelError(string.Empty, "Fail to save!");
			}

			return View(model);
		}

		/// <summary>
		/// Edit item
		/// </summary>
		/// <param name="itemId"></param>
		/// <returns></returns>
		[HttpGet]
		public async Task<IActionResult> EditItem(Guid itemId)
		{
			ViewBag.Routes = _context.Pages.Where(x => !x.IsDeleted && !x.IsLayout).Select(x => x.Path);
			var item = await _dataService.GetByIdSystem<MenuItem, MenuItem>(itemId);
			if (!item.IsSuccess) return NotFound();
			return View(item.Result);
		}

		/// <summary>
		/// Edit item
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		public async Task<IActionResult> EditItem(MenuItem model)
		{
			var rq = await _dataService.UpdateSystem(model);
			if (rq.IsSuccess)
			{
				return RedirectToAction("GetMenu", new
				{
					model.MenuId,
					ParentId = model.ParentMenuItemId
				});
			}

			ViewBag.Routes = _context.Pages.Where(x => !x.IsDeleted && !x.IsLayout).Select(x => x.Path);
			ModelState.AddModelError(string.Empty, "Fail to save!");
			return View(model);
		}

		/// <summary>
		/// Load menu items
		/// </summary>
		/// <param name="param"></param>
		/// <param name="menuId"></param>
		/// <param name="parentId"></param>
		/// <returns></returns>
		[HttpPost]
		public async Task<JsonResult> LoadMenuItems(DTParameters param, Guid menuId, Guid? parentId = null)
		{
			var filtered = await _dataService.Filter<MenuItem>(param.Search.Value, param.SortOrder, param.Start,
				param.Length, x => x.MenuId.Equals(menuId) && x.ParentMenuItemId.Equals(parentId));

			var finalResult = new DTResult<MenuItem>
			{
				draw = param.Draw,
				data = filtered.Item1,
				recordsFiltered = filtered.Item2,
				recordsTotal = filtered.Item1.Count()
			};
			return Json(finalResult);
		}

		/// <summary>
		/// Load page types with ajax
		/// </summary>
		/// <param name="param"></param>
		/// <returns></returns>
		[HttpPost]
		[AjaxOnly]
		public async Task<JsonResult> LoadPages(DTParameters param)
		{
			var filtered = await _dataService.Filter<Menu>(param.Search.Value, param.SortOrder, param.Start,
				param.Length);

			var finalResult = new DTResult<Menu>
			{
				draw = param.Draw,
				data = filtered.Item1,
				recordsFiltered = filtered.Item2,
				recordsTotal = filtered.Item1.Count()
			};
			return Json(finalResult);
		}

		/// <summary>
		/// Delete page type by id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[Route("api/[controller]/[action]")]
		[ValidateAntiForgeryToken]
		[HttpPost, Produces("application/json", Type = typeof(ResultModel))]
		public async Task<JsonResult> Delete(string id)
		{
			if (string.IsNullOrEmpty(id)) return Json(new { message = "Fail to delete menu!", success = false });
			var menu = await _dataService.DeletePermanent<Menu>(Guid.Parse(id));
			if (!menu.IsSuccess) return Json(new { message = "Fail to delete menu!", success = false });

			return Json(new { message = "Menu was delete with success!", success = true });
		}


		/// <summary>
		/// Delete page type by id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[Route("api/[controller]/[action]")]
		[ValidateAntiForgeryToken]
		[HttpPost, Produces("application/json", Type = typeof(ResultModel))]
		public async Task<JsonResult> DeleteMenuItem(string id)
		{
			if (string.IsNullOrEmpty(id)) return Json(new { message = "Fail to delete menu item!", success = false });
			var menu = await _dataService.DeletePermanent<MenuItem>(Guid.Parse(id));
			if (!menu.IsSuccess) return Json(new { message = "Fail to delete menu item!", success = false });

			return Json(new { message = "Model was delete with success!", success = true });
		}
	}
}