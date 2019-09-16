﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ST.Core;
using ST.Dashboard.Abstractions;

namespace ST.Dashboard.Razor.Controllers
{
    [Authorize(Roles = GlobalResources.Roles.ADMINISTRATOR)]
    public class DashboardController : Controller
    {
        /// <summary>
        /// Inject dashboard manager
        /// </summary>
        private readonly IDashboardManager _dashboardManager;

        public DashboardController(IDashboardManager dashboardManager)
        {
            _dashboardManager = dashboardManager;
        }

        /// <summary>
        /// Index page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Builder()
        {
            return View();
        }
    }
}