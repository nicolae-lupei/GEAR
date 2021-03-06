using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using GR.Core;
using GR.Core.Helpers;
using GR.Core.Razor.BaseControllers;
using GR.Identity.Abstractions;
using GR.Process.Razor.ViewModels.ProcessViewModels;
using GR.Processes.Abstractions;
using GR.Processes.Abstractions.Models;

namespace GR.Process.Razor.Controllers
{
    public class ProcessController : BaseGearController
    {
        #region Injectable

        /// <summary>
        /// Inject process parser
        /// </summary>
        private readonly IProcessParser _processParser;

        /// <summary>
        /// Context
        /// </summary>
        private IProcessesDbContext Context { get; }

        /// <summary>
        /// Inject logger
        /// </summary>
        private readonly ILogger<ProcessController> _logger;

        /// <summary>
        /// Inject identity context
        /// </summary>
        private readonly IIdentityContext _applicationDbContext;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="logger"></param>
        /// <param name="applicationDbContext"></param>
        /// <param name="processParser"></param>
        public ProcessController(IProcessesDbContext context,
            ILogger<ProcessController> logger, IIdentityContext applicationDbContext, IProcessParser processParser)
        {
            Context = context;
            _logger = logger;
            _applicationDbContext = applicationDbContext;
            _processParser = processParser;
        }

        /// <summary>
        /// Create process
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Create() => View();

        /// <summary>
        /// Save process
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProcessViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var settings = JsonConvert.DeserializeObject<IEnumerable<Dictionary<string, string>>>(model.DiagramSettings);
            _processParser.Init(model.Diagram, settings);
            var processes = _processParser.GetProcesses().ToList();

            if (!processes.Any())
            {
                ModelState.AddModelError(string.Empty, "No identified processes in your diagram, try to modify as system standard");
                return View(model);
            }

            var newProcessDiagram = new STProcessSchema
            {
                Changed = DateTime.Now,
                Created = DateTime.Now,
                ModifiedBy = User.Identity.Name,
                Title = model.Title,
                Diagram = model.Diagram.Replace("'+'", string.Empty),
                Author = _applicationDbContext.Users.Single(x => x.UserName.Equals(User.Identity.Name)).UserName
            };

            try
            {
                await Context.ProcessSchemas.AddAsync(newProcessDiagram);
                foreach (var process in processes)
                {
                    process.ProcessSchema = newProcessDiagram;
                }
                await Context.Processes.AddRangeAsync(processes);
                await Context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Delete process
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Delete(Guid? id)
        {
            if (!id.HasValue)
            {
                return Json(new { success = false, message = "Id not found" });
            }

            var processDiagrams = Context.ProcessSchemas.AsNoTracking().SingleOrDefault(x => x.Id == id);
            if (processDiagrams == null)
            {
                return Json(new { success = false, message = "Process Diagram not found" });
            }

            try
            {
                Context.ProcessSchemas.Remove(processDiagrams);
                Context.SaveChanges();
                return Json(new { success = true, message = "Diagram deleted!" });
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return Json(new { success = false, message = "Error on delete!" });
            }
        }

        /// <summary>
        /// Edit process
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await Context.ProcessSchemas.FirstOrDefaultAsync(x => x.Id.Equals(Guid.Parse(id)));
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        /// <summary>
        /// Save changes
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(STProcessSchema model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.ModifiedBy = User.Identity.Name;
            model.Changed = DateTime.Now;
            try
            {
                Context.ProcessSchemas.Update(model);
                await Context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Get config for bpm diagram creator
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetBpmConfig()
        {
            var path = Path.Combine(AppContext.BaseDirectory, "Configuration/bpmConfig.json");
            var config = JsonParser.ReadArrayDataFromJsonFile<List<object>>(path);
            return Json(config);
        }

        /// <summary>
        /// Process diagram list
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ProcessDiagramsList(DTParameters param)
        {
            var filtered = GetProcessDiagramsFiltered(param.Search.Value, param.SortOrder, param.Start, param.Length,
                out var totalCount);
            var orderList = filtered.Select(o => new ProcessesListViewModel
            {
                Id = o.Id,
                Title = o.Title,
                Synchronized = o.Synchronized,
                IsDeleted = o.IsDeleted,
                //AuthorName = _applicationDbContext.Users.Find(o.Author).Name,
                CreatedString = o.Created.ToShortDateString(),
                ChangedString = o.Changed.ToShortDateString(),
                //ModifiedBy = _applicationDbContext.Users.Find(o.ModifiedBy).Name,
            });
            var finalResult = new DTResult<ProcessesListViewModel>
            {
                Draw = param.Draw,
                Data = orderList.ToList(),
                RecordsFiltered = totalCount,
                RecordsTotal = filtered.Count
            };

            return Json(finalResult);
        }

        /// <summary>
        /// Get Process Diagrams filtered
        /// </summary>
        /// <param name="search"></param>
        /// <param name="sortOrder"></param>
        /// <param name="start"></param>
        /// <param name="length"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        private List<STProcessSchema> GetProcessDiagramsFiltered(string search, string sortOrder, int start, int length,
            out int totalCount)
        {
            var result = Context.ProcessSchemas.AsNoTracking().Where(p =>
                search == null || p.Title != null &&
                p.Title.ToLower().Contains(search.ToLower()) || p.Author != null &&
                p.Author.ToLower().Contains(search.ToLower()) || p.ModifiedBy != null &&
                p.ModifiedBy.ToString().ToLower().Contains(search.ToLower()) || p.Created != null &&
                p.Created.ToString(CultureInfo.InvariantCulture).ToLower().Contains(search.ToLower())).ToList();
            totalCount = result.Count;

            result = result.Skip(start).Take(length).ToList();
            switch (sortOrder)
            {
                case "title":
                    result = result.OrderBy(a => a.Title).ToList();
                    break;
                case "synchronized":
                    result = result.OrderBy(a => a.Synchronized).ToList();
                    break;
                case "created":
                    result = result.OrderBy(a => a.Created).ToList();
                    break;
                case "author":
                    result = result.OrderBy(a => a.Author).ToList();
                    break;
                case "modifiedBy":
                    result = result.OrderBy(a => a.ModifiedBy).ToList();
                    break;
                case "changed":
                    result = result.OrderBy(a => a.Changed).ToList();
                    break;
                case "isDeleted":
                    result = result.OrderBy(a => a.IsDeleted).ToList();
                    break;
                case "title DESC":
                    result = result.OrderByDescending(a => a.Title).ToList();
                    break;
                case "created DESC":
                    result = result.OrderByDescending(a => a.Created).ToList();
                    break;
                case "author DESC":
                    result = result.OrderByDescending(a => a.Author).ToList();
                    break;
                case "modifiedBy DESC":
                    result = result.OrderByDescending(a => a.ModifiedBy).ToList();
                    break;
                case "synchronized DESC":
                    result = result.OrderByDescending(a => a.Synchronized).ToList();
                    break;
                case "changed DESC":
                    result = result.OrderByDescending(a => a.Changed).ToList();
                    break;
                case "isDeleted DESC":
                    result = result.OrderByDescending(a => a.IsDeleted).ToList();
                    break;
                default:
                    result = result.AsQueryable().ToList();
                    break;
            }

            return result.ToList();
        }

        /// <summary>
        /// Get Processes
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }
    }
}