﻿using GR.Core;
using GR.Identity.Abstractions.Helpers.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GR.Logger.Razor.Controllers
{
    [Authorize]
    [GearAuthorize(GlobalResources.Roles.ADMINISTRATOR)]
    public class LogsController : Controller
    {
        /// <summary>
        /// View with logs
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}