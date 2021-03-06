﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GR.Calendar.Abstractions;
using GR.Calendar.Abstractions.Models;
using GR.Calendar.Abstractions.Models.ViewModels;
using GR.Core.Extensions;
using GR.Core.Helpers;
using GR.Identity.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using GR.Calendar.Abstractions.Enums;
using GR.Calendar.Abstractions.ExternalProviders;
using GR.Calendar.Abstractions.Helpers.Mappers;
using GR.Calendar.Abstractions.Helpers.ServiceBuilders;
using GR.Calendar.Razor.ViewModels;
using GR.Identity.Abstractions.Models.MultiTenants;
using GR.Identity.Abstractions.ViewModels.UserViewModels;
using GR.MultiTenant.Abstractions;

namespace GR.Calendar.Razor.Controllers
{
    [Authorize]
    public sealed class CalendarController : Controller
    {
        #region Injectable
        /// <summary>
        /// Inject Task service
        /// </summary>
        private readonly ICalendarManager _calendarManager;

        /// <summary>
        /// Inject user manager
        /// </summary>
        private readonly IUserManager<GearUser> _userManager;

        /// <summary>
        /// Inject organization service
        /// </summary>
        private readonly IOrganizationService<Tenant> _organizationService;

        /// <summary>
        /// Settings
        /// </summary>
        private readonly JsonSerializerSettings _serializeSettings;

        /// <summary>
        /// Inject signin manager
        /// </summary>
        private readonly SignInManager<GearUser> _signInManager;

        /// <summary>
        /// Inject token provider
        /// </summary>
        private readonly ICalendarExternalTokenProvider _externalTokenProvider;

        /// <summary>
        /// Inject user settings 
        /// </summary>
        private readonly ICalendarUserSettingsService _userSettingsService;

        #endregion

        public CalendarController(ICalendarManager calendarManager, IUserManager<GearUser> userManager, IOrganizationService<Tenant> organizationService, SignInManager<GearUser> signInManager, ICalendarExternalTokenProvider externalTokenProvider, ICalendarUserSettingsService userSettingsService)
        {
            _calendarManager = calendarManager;
            _userManager = userManager;
            _organizationService = organizationService;
            _signInManager = signInManager;
            _externalTokenProvider = externalTokenProvider;
            _userSettingsService = userSettingsService;
            _serializeSettings = CalendarServiceCollection.JsonSerializerSettings;
        }

        /// <summary>
        /// Internal calendar
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Add new event
        /// </summary>
        /// <param name="newEvent"></param>
        /// <returns></returns>
        [HttpPost, Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<Guid>))]
        public async Task<JsonResult> AddEvent([Required]BaseEventViewModel newEvent)
        {
            var response = new ResultModel<Guid>();
            if (ModelState.IsValid) return Json(await _calendarManager.AddEventAsync(newEvent));
            response.Errors = ModelState.ToResultModelErrors().ToList();
            return Json(response, _serializeSettings);
        }

        /// <summary>
        /// Update event
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        public async Task<JsonResult> UpdateEvent([Required]UpdateEventViewModel model)
        {
            var response = new ResultModel();
            if (ModelState.IsValid)
            {
                var userRequest = await _userManager.GetCurrentUserAsync();
                if (!userRequest.IsSuccess) return Json(response);
                var user = userRequest.Result;
                return Json(await _calendarManager.UpdateEventAsync(model, user.Id));
            }
            response.Errors = ModelState.ToResultModelErrors().ToList();
            return Json(response, _serializeSettings);
        }


        /// <summary>
        /// Change member acceptance
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="memberId"></param>
        /// <param name="acceptance"></param>
        /// <returns></returns>
        [HttpPost, Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        public async Task<JsonResult> ChangeMemberEventAcceptance(Guid? eventId, Guid? memberId, EventAcceptance acceptance = EventAcceptance.Tentative)
        {
            return Json(await _calendarManager.ChangeMemberEventAcceptanceAsync(eventId, memberId, acceptance));
        }


        /// <summary>
        /// Get my events
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<IEnumerable<GetEventViewModel>>))]
        public async Task<JsonResult> GetAllEventsOrganizedByMe()
        {
            var eventRequest = await _calendarManager.GetAllEventsOrganizedByMeAsync();
            return Json(EventMapper.MapWithHelpers(eventRequest), _serializeSettings);
        }

        /// <summary>
        /// Get user event on interval for current organization
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [HttpGet, Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<IEnumerable<GetEventViewModel>>))]
        public async Task<JsonResult> GetOrganizationUserEvents(Guid? userId, [Required]DateTime startDate, [Required]DateTime endDate)
        {
            var response = new ResultModel<IEnumerable<CalendarEvent>>();
            if (!await _organizationService.IsUserPartOfOrganizationAsync(userId, _userManager.CurrentUserTenantId))
            {
                response.Errors.Add(new ErrorModel(string.Empty, "User not found or not assigned to this organization"));
                return Json(response);
            }

            var eventRequest = await _calendarManager.GetEventsAsync(userId, startDate, endDate);
            return Json(EventMapper.Map(eventRequest), _serializeSettings);
        }

        /// <summary>
        /// Get my events on by start and end date
        /// </summary>
        /// <param name="startDate"></param>, 
        /// <param name="endDate"></param>, 
        /// <returns></returns>
        [HttpGet, Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<IEnumerable<GetEventViewModel>>))]
        public async Task<JsonResult> GetMyEvents([Required]DateTime startDate, [Required]DateTime endDate)
        {
            var eventRequest = await _calendarManager.GetMyEventsAsync(startDate, endDate);
            return Json(EventMapper.MapWithHelpers(eventRequest), _serializeSettings);
        }

        /// <summary>
        /// Get events by time line
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="origin"></param>
        /// <param name="timeLineType"></param>
        /// <param name="expandDayPrecision"></param>
        /// <returns></returns>
        [HttpGet, Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<IEnumerable<GetEventViewModel>>))]
        public async Task<JsonResult> GetUserEventsByTimeLine(Guid? userId, DateTime? origin,
            CalendarTimeLineType timeLineType = CalendarTimeLineType.Month, int expandDayPrecision = 0)
        {
            var response = new ResultModel<IEnumerable<GetEventViewModel>>();
            if (!await _organizationService.IsUserPartOfOrganizationAsync(userId, _userManager.CurrentUserTenantId))
            {
                response.Errors.Add(new ErrorModel(string.Empty, "User not found or not assigned to this organization"));
                return Json(response);
            }

            var eventRequest = await _calendarManager.GetUserEventsByTimeLineAsync(userId, origin, timeLineType, expandDayPrecision);
            return Json(EventMapper.MapWithHelpers(eventRequest), _serializeSettings);
        }

        /// <summary>
        /// Get event by Id
        /// </summary>
        /// <param name="eventId">Event id</param>
        /// <returns></returns>
        [HttpGet, Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<GetEventViewModel>))]
        public async Task<JsonResult> GetEventById([Required] Guid? eventId)
        {
            var eventRequest = await _calendarManager.GetEventByIdAsync(eventId);
            return Json(EventMapper.Map(eventRequest), _serializeSettings);
        }

        /// <summary>
        /// Delete event permanently
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>

        [HttpDelete, Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        public async Task<JsonResult> DeletePermanently(Guid? eventId) => Json(await _calendarManager.DeleteEventPermanentlyAsync(eventId));

        /// <summary>
        /// Delete event
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>

        [HttpDelete, Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        public async Task<JsonResult> DeleteLogically(Guid? eventId) => Json(await _calendarManager.DeleteEventLogicallyAsync(eventId));


        /// <summary>
        /// Restore event
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>

        [HttpDelete, Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        public async Task<JsonResult> Restore(Guid? eventId) => Json(await _calendarManager.RestoreEventLogicallyAsync(eventId));


        /// <summary>
        /// Helpers
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<Dictionary<string, Dictionary<int, Dictionary<string, string>>>>))]
        public JsonResult Helpers() => Json(new ResultModel<Dictionary<string, Dictionary<int, Dictionary<string, string>>>>
        {
            IsSuccess = true,
            Result = new GetEventWithHelpersViewModel().Helpers
        });


        /// <summary>
        /// Get users
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<IEnumerable<UserInfoViewModel>>))]
        public JsonResult GetOrganizationUsers()
        {
            var users = _organizationService.GetAllowedUsersByOrganizationId(_userManager.CurrentUserTenantId.GetValueOrDefault());
            return Json(new ResultModel
            {
                IsSuccess = true,
                Result = users.Select(x => new UserInfoViewModel(x))
            });
        }

        /// <summary>
        /// Enable provider
        /// </summary>
        /// <param name="providerName"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [HttpPost, Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        public async Task<JsonResult> EnableProvider(string providerName, bool state)
        {
            var user = await _userManager.GetCurrentUserAsync();
            return !user.IsSuccess ? Json(new ResultModel())
                : Json(await _userSettingsService.ChangeProviderSettings(user.Result.Id, providerName, state));
        }

        /// <summary>
        /// Calendar providers
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ExternalCalendarProviders()
        {
            var user = await _userManager.GetCurrentUserAsync();
            if (!user.IsSuccess) return NotFound();
            var userId = user.Result.Id;
            var factory = new ExternalCalendarProviderFactory();
            var providers = factory.GetProvidersInfo();
            var exportModel = new List<ExternalCalendarProviderSettingsViewModel>();

            foreach (var provider in providers)
            {
                var providerConf = new ExternalCalendarProviderSettingsViewModel
                {
                    ProviderName = provider.ProviderName,
                    FontAwesomeIcon = provider.FontAwesomeIcon,
                    DisplayName = provider.DisplayName
                };

                var isEnabledRequest = await _userSettingsService.IsProviderEnabledAsync(userId, provider.ProviderName);
                providerConf.IsEnabled = isEnabledRequest.IsSuccess;

                if (!isEnabledRequest.IsSuccess)
                {
                    exportModel.Add(providerConf); continue;
                }

                var providerService = factory.CreateService(provider.ProviderName);
                var authResult = await providerService.AuthorizeAsync(userId);
                if (authResult.IsSuccess)
                {
                    var provideruserRequest = await providerService.GetUserAsync();
                    if (provideruserRequest.IsSuccess)
                    {
                        var providerUser = provideruserRequest.Result;
                        providerConf.User = new ExternalCalendarUser
                        {
                            IsAuthorized = true,
                            UserName = providerUser.DisplayName,
                            EmailAdress = providerUser.EmailAddress,
                            ImageUrl = providerUser.ImageUrl
                        };
                    }
                }
                exportModel.Add(providerConf);
            }

            return View(exportModel);
        }


        /// <summary>
        /// External login
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CalendarExternalLogin(string provider, string returnUrl = null)
        {
            var user = await _userManager.GetCurrentUserAsync();
            if (!user.IsSuccess) return NotFound();
            var factory = new ExternalCalendarProviderFactory();
            IActionResult result;
            switch (provider)
            {
                case "GoogleCalendarProvider":
                    {

                        if (factory.GetProviders().Contains(provider))
                        {
                            var serviceProvider = factory.CreateService(provider);
                            await serviceProvider.AuthorizeAsync(user.Result.Id, true);
                            result = RedirectToAction("ExternalCalendarProviders");
                        }
                        else result = RedirectToAction(nameof(ExternalCalendarProviders));
                    }
                    break;
                case "OutlookCalendarProvider":
                    {
                        var redirectUrl = Url.Action(nameof(OutlookCalendarExternalLoginCallback), "Calendar", new
                        {
                            returnUrl,
                            gearUserId = user.Result.Id,
                            provider
                        });

                        const string instanceProvider = "Microsoft";

                        var properties = _signInManager.ConfigureExternalAuthenticationProperties(instanceProvider, redirectUrl);

                        result = Challenge(properties, instanceProvider);
                    }
                    break;

                default:
                    {
                        result = RedirectToAction(nameof(ExternalCalendarProviders));
                    }
                    break;
            }

            return result;
        }

        /// <summary>
        /// External login call back
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <param name="remoteError"></param>
        /// <param name="gearUserId"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> OutlookCalendarExternalLoginCallback(string returnUrl = null, string remoteError = null, Guid? gearUserId = null)
        {
            if (remoteError != null)
            {
                return RedirectToAction();
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null) return RedirectToAction(nameof(ExternalCalendarProviders));

            await _externalTokenProvider.SetTokenAsync(new ExternalProviderToken
            {
                ProviderName = "OutlookCalendarProvider",
                UserId = gearUserId.GetValueOrDefault(),
                Value = info.AuthenticationTokens.SerializeAsJson()
            });

            return RedirectToAction(nameof(ExternalCalendarProviders));
        }
    }
}