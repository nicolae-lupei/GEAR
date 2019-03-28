using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ST.BaseBusinessRepository;
using ST.CORE.Attributes;
using ST.Entities.Data;
using ST.Entities.Models.Forms;
using ST.Entities.Models.Tables;
using ST.Entities.Services.Abstraction;
using ST.Entities.ViewModels.Form;
using ST.Identity.Attributes;
using ST.Identity.Data.Permissions;
using ST.Identity.Data.UserProfiles;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ST.CORE.ViewModels;
using ST.CORE.ViewModels.FormsViewModels;
using ST.DynamicEntityStorage.Extensions;
using Settings = ST.Configuration.Settings;

namespace ST.CORE.Controllers.Entity
{
	/// <inheritdoc />
	/// <summary>
	/// Forms manipulation
	/// </summary>
	[Authorize]
	public class FormController : Controller
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="repository"></param>
		/// <param name="formService"></param>
		/// <param name="userManager"></param>
		/// <param name="entitiesDbContext"></param>
		public FormController(IBaseBusinessRepository<EntitiesDbContext> repository,
			IFormService formService, UserManager<ApplicationUser> userManager, EntitiesDbContext entitiesDbContext)
		{
			Repository = repository;
			FormService = formService;
			_userManager = userManager;
			UserManager = userManager;
			_entitiesDbContext = entitiesDbContext;
		}

		#region Import
		private readonly UserManager<ApplicationUser> _userManager;

		private UserManager<ApplicationUser> UserManager { get; }
		private readonly EntitiesDbContext _entitiesDbContext;

		private IFormService FormService { get; }

		private IBaseBusinessRepository<EntitiesDbContext> Repository { get; }
		#endregion

		/// <summary>
		/// Create new form
		/// </summary>
		/// <returns></returns>
		public IActionResult Create()
		{
			ViewData["models"] = Repository.GetAll<TableModel>(x => x.IsDeleted == false);
			ViewData["formTypes"] = Repository.GetAll<FormType>(x => x.IsDeleted == false).OrderBy(s => s.Code);
			return View();
		}

		/// <summary>
		/// Preview form by model
		/// </summary>
		/// <param name="mode"></param>
		/// <param name="tableId"></param>
		/// <param name="formType"></param>
		/// <returns></returns>
		public IActionResult Generate(string mode, Guid tableId, Guid formType)
		{
			ViewBag.FormType = Repository.GetSingle<FormType>(formType);
			return View();
		}

		/// <summary>
		/// Get form for update
		/// </summary>
		/// <param name="formId"></param>
		/// <returns></returns>
		[HttpGet]
		public IActionResult Edit(Guid formId)
		{
			var form = _entitiesDbContext.Forms
				.Include(x => x.Type)
				.FirstOrDefault(x => x.Id.Equals(formId));
			if (form == null) return NotFound();
			ViewBag.Form = form;

			return View();
		}

		/// <summary>
		/// Create new form
		/// </summary>
		/// <param name="form"></param>
		/// <param name="formId"></param>
		/// <param name="name"></param>
		/// <param name="description"></param>
		/// <param name="postUrl"></param>
		/// <param name="redirectUrl"></param>
		/// <returns></returns>
		[Route("api/[controller]/[action]")]
		[HttpPost, Produces("application/json", Type = typeof(ResultModel))]
		public async Task<JsonResult> UpdateForm(FormViewModel form, [Required]Guid formId,
			string name, string description, string postUrl, string redirectUrl)
		{
			var user = await _userManager.GetUserAsync(User);
			var bdForm = _entitiesDbContext.Forms.FirstOrDefault(x => x.Id.Equals(formId));
			if (bdForm == null) return Json(new ResultModel());
			var res = FormService.DeleteForm(formId);
			if (!res.IsSuccess) return Json(new ResultModel());
			var response = FormService.CreateForm(new FormCreateDetailsViewModel
			{
				Id = formId,
				Created = bdForm.Created,
				Author = bdForm.Author,
				Description = description,
				Name = name,
				PostUrl = postUrl,
				RedirectUrl = redirectUrl,
				Model = form,
				TableId = bdForm.TableId,
				FormTypeId = bdForm.TypeId,
				EditMode = true
			}, user.Id);

			return Json(response);
		}

		/// <summary>
		/// Get Form by id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[Route("api/[controller]/[action]")]
		[HttpGet, Produces("application/json", Type = typeof(ResultModel))]
		public JsonResult GetForm(Guid id)
		{
			var response = FormService.GetFormById(id);
			return Json(response, new JsonSerializerSettings
			{
				NullValueHandling = NullValueHandling.Ignore
			});
		}

		/// <summary>
		/// Get table fields for preview form
		/// </summary>
		/// <param name="tableId"></param>
		/// <returns></returns>
		[Route("api/[controller]/[action]")]
		[HttpGet, Produces("application/json", Type = typeof(ResultModel))]
		public JsonResult GetTableFields(Guid tableId)
		{
			var fields = _entitiesDbContext.TableFields
				.Include(x => x.TableFieldConfigValues)
				.Where(x => x.TableId == tableId);

			return new JsonResult(new ResultModel
			{
				IsSuccess = true,
				Result = fields.ToList()
			}, new JsonSerializerSettings
			{
				Formatting = Formatting.Indented,
				ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
				ContractResolver = new CamelCasePropertyNamesContractResolver()
			});
		}

		/// <summary>
		/// Get by page
		/// </summary>
		/// <returns></returns>
		[AuthorizePermission(PermissionsConstants.CorePermissions.BpmFormRead)]
		public IActionResult Index()
		{
			return View();
		}

		/// <summary>
		/// Preview form
		/// </summary>
		/// <param name="formId"></param>
		/// <returns></returns>
		public IActionResult Preview(Guid formId)
		{
			ViewBag.FormId = formId;
			ViewBag.Form = _entitiesDbContext.Forms.FirstOrDefault(x => x.Id == formId);
			ViewBag.FormType = FormService.GetTypeByFormId(formId);
			return View();
		}

		/// <summary>
		/// Load forms with ajax
		/// </summary>
		/// <param name="param"></param>
		/// <param name="entityId"></param>
		/// <returns></returns>
		[HttpPost]
		[AjaxOnly]
		public JsonResult LoadForms(DTParameters param, Guid entityId)
		{
			var filtered = _entitiesDbContext.Filter<Form>(param.Search.Value, param.SortOrder, param.Start,
				param.Length,
				out var totalCount, x => (entityId != Guid.Empty && x.TableId == entityId) || entityId == Guid.Empty);


			var finalResult = new DTResult<FormListViewModel>
			{
				draw = param.Draw,
				data = filtered.Select(x => new FormListViewModel
				{
					Id = x.Id,
					Name = x.Name,
					Created = x.Created,
					TableName = _entitiesDbContext.Table.FirstOrDefault(o => o.Id == x.TableId)?.Name,
					IsDeleted = x.IsDeleted,
					TypeId = x.TypeId,
					Type = x.Type,
					Description = x.Description,
					Author = UserManager.Users.FirstOrDefault(y => y.Id.Equals(x.Author))?.Name,
					Changed = x.Changed,
					Table = x.Table,
					ModifiedBy = x.ModifiedBy,
					TableId = x.TableId
				}).ToList(),
				recordsFiltered = totalCount,
				recordsTotal = filtered.Count
			};
			return Json(finalResult);
		}

		/// <summary>
		/// Create new form
		/// </summary>
		/// <param name="form"></param>
		/// <param name="tableId"></param>
		/// <param name="formTypeId"></param>
		/// <param name="name"></param>
		/// <param name="description"></param>
		/// <param name="postUrl"></param>
		/// <param name="redirectUrl"></param>
		/// <returns></returns>
		[Route("api/[controller]/[action]")]
		[HttpPost, Produces("application/json", Type = typeof(ResultModel))]
		public async Task<JsonResult> CreateNewForm(FormViewModel form, Guid tableId, Guid formTypeId,
			string name, string description, string postUrl, string redirectUrl)
		{
			var user = await _userManager.GetUserAsync(User);
			var response = FormService.CreateForm(new FormCreateDetailsViewModel
			{
				Description = description,
				Name = name,
				PostUrl = postUrl,
				RedirectUrl = redirectUrl,
				Model = form,
				TableId = tableId,
				FormTypeId = formTypeId
			}, user.Id);
			return Json(response);
		}

		/// <summary>
		/// Get entity fields
		/// </summary>
		/// <param name="tableId"></param>
		/// <returns></returns>
		[HttpGet]
		[Authorize(Roles = Settings.SuperAdmin)]
		public JsonResult GetEntityFields(Guid tableId)
		{
			var fields = _entitiesDbContext.Table
				.Include(x => x.TableFields)
				.FirstOrDefault(x => !x.IsDeleted && x.Id == tableId)?.TableFields
				.Select(x => new
				{
					x.Id,
					x.Name,
					x.DataType
				})
				.ToList();

			return new JsonResult(fields);
		}

		/// <summary>
		/// Get entity reference fields
		/// </summary>
		/// <param name="entityName"></param>
		/// <param name="entitySchema"></param>
		/// <returns></returns>
		[HttpGet]
		[Authorize(Roles = Settings.SuperAdmin)]
		public JsonResult GetEntityReferenceFields(string entityName, string entitySchema)
		{
			var table = _entitiesDbContext.Table.Include(x => x.TableFields).FirstOrDefault(x => x.Name == entityName && x.EntityType == entitySchema);
			if (table == null) return Json(default(Collection<TableModelFields>));
			return Json(table.TableFields.Select(x => new
			{
				x.DataType,
				x.Id,
				x.Name
			}));
		}

		/// <summary>
		/// Delete form by id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[Route("api/[controller]/[action]")]
		[ValidateAntiForgeryToken]
		[HttpPost, Produces("application/json", Type = typeof(ResultModel))]
		public JsonResult Delete(string id)
		{
			if (string.IsNullOrEmpty(id)) return Json(new { message = "Fail to delete form!", success = false });
			var res = FormService.DeleteForm(Guid.Parse(id));
			return Json(new { message = "Form was delete with success!", success = res.IsSuccess });
		}
	}
}