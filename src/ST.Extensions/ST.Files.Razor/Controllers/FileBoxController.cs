﻿using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ST.Core.Helpers;
using ST.Files.Abstraction.Models.ViewModels;
using ST.Files.Box.Abstraction;

namespace ST.Files.Razor.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    public sealed class FileBoxController : Controller
    {
        /// <summary>
        /// Inject file service
        /// </summary>
        private readonly IFileBoxManager _fileManager;

        public FileBoxController(IFileBoxManager fileManager)
        {
            _fileManager = fileManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Get file
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public FileResult GetFile(Guid id)
        {
            var response = _fileManager.GetFileById(id);
            return response.IsSuccess ? PhysicalFile(Path.Combine(response.Result.Path, response.Result.FileName), "application/octet-stream", response.Result.FileName) : null;
        }

        /// <summary>
        /// Upload/Update file
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Produces("application/json", Type = typeof(ResultModel))]
        public JsonResult Upload(Guid id)
        {
            var file = new UploadFileViewModel
            {
                File = Request.Form.Files.FirstOrDefault(),
                Id = id
            };
            var response = _fileManager.AddFile(file);
            return Json(response);
        }

        /// <summary>
        /// Multiple Upload
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Produces("application/json", Type = typeof(ResultModel))]
        public JsonResult UploadMultiple()
        {
            var response = Request.Form.Files.Select(item => new UploadFileViewModel
            {
                File = item,
                Id = Guid.Empty
            }).Select(file => _fileManager.AddFile(file)).ToList();

            return Json(response);
        }

        /// <summary>
        /// Delete file logical
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Produces("application/json", Type = typeof(ResultModel))]
        public JsonResult Delete(Guid id)
        {
            if (id != Guid.Empty) return Json(new {message = "Fail to delete file!", success = false});

            var response = _fileManager.DeleteFile(id);
            return Json(response);
        }

        /// <summary>
        /// Restore File
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Produces("application/json", Type = typeof(ResultModel))]
        public JsonResult Restore(Guid id)
        {
            if (id != Guid.Empty) return Json(new {message = "Fail to restore file!", success = false});

            var response = _fileManager.RestoreFile(id);
            return Json(response);
        }

        /// <summary>
        /// Delete file permanently
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Produces("application/json", Type = typeof(ResultModel))]
        public JsonResult DeletePermanent(Guid id)
        {
            if (id != Guid.Empty) return Json(new {message = "Fail to delete form!", success = false});

            var response = _fileManager.DeleteFilePermanent(id);
            return Json(response);
        }
    }
}