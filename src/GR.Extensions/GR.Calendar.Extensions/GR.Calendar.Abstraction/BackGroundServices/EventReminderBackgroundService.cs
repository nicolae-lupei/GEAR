﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using GR.Calendar.Abstractions.Models;
using GR.Core;
using GR.Core.Extensions;
using GR.Core.Helpers;
using GR.Core.Services;
using GR.Identity.Abstractions;
using GR.Notifications.Abstractions;
using GR.Notifications.Abstractions.Models.Notifications;

namespace GR.Calendar.Abstractions.BackGroundServices
{
    public class EventReminderBackgroundService : BaseBackgroundService<EventReminderBackgroundService>
    {
        #region Injectable
        /// <summary>
        /// Inject calendar manager
        /// </summary>
        private readonly ICalendarDbContext _calendarDbContext;

        /// <summary>
        /// Notifier
        /// </summary>
        private readonly INotify<GearRole> _notify;
        #endregion


        public EventReminderBackgroundService(ILogger<EventReminderBackgroundService> logger, ICalendarDbContext calendarDbContext, INotify<GearRole> notify)
            : base("CalendarEvents", logger)
        {
            _calendarDbContext = calendarDbContext;
            _notify = notify;
            Interval = TimeSpan.FromMinutes(15);
        }

        /// <summary>
        /// Send logs
        /// </summary>
        /// <param name="state"></param>
        public override async Task Execute(object state)
        {
            if (!GearApplication.Configured) return;
            var events = await GetEventsForAllUsersWhatStartInAsync(59);
            if (!events.IsSuccess) return;
            var now = DateTime.Now;
            foreach (var evt in events.Result)
            {
                var minutes = (evt.StartDate - now).Minutes;
                if (evt.MinutesToRemind > minutes) continue;
                var users = new List<Guid> { evt.Organizer };
                users.AddRange(evt.EventMembers.Select(x => x.UserId));
                await _notify.SendNotificationAsync(users, NotificationType.Info, evt.Title, $"This event will take place over {minutes} minutes");
                evt.RemindSent = true;
                _calendarDbContext.CalendarEvents.Update(evt);
                await _calendarDbContext.PushAsync();
            }
        }

        /// <summary>
        /// Get today event for all users
        /// </summary>
        /// <returns></returns>
        private async Task<ResultModel<IEnumerable<CalendarEvent>>> GetEventsForAllUsersWhatStartInAsync(int minutes = 15)
        {
            var response = new ResultModel<IEnumerable<CalendarEvent>>();
            var now = DateTime.Now;
            var events = await _calendarDbContext.CalendarEvents
                .Include(x => x.EventMembers)
                .Where(x => (x.StartDate - now).TotalMinutes <= minutes && (x.StartDate - now).TotalMinutes > 0 && !x.RemindSent)
                .ToListAsync();

            response.IsSuccess = true;
            response.Result = events;
            return response;
        }
    }
}