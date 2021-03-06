﻿using System;
using System.Collections.Generic;

namespace GR.Calendar.Abstractions.Events.EventArgs
{
    public class CalendarEventCreatedEventArgs : System.EventArgs
    {
        public virtual Guid? EventId { get; set; }
        /// <summary>
        /// Event title
        /// </summary>
        public virtual string Title { get; set; }

        /// <summary>
        /// Details
        /// </summary>
        public virtual string Details { get; set; }

        /// <summary>
        /// Organizer
        /// </summary>
        public virtual string Organizer { get; set; }

        /// <summary>
        /// Invited users
        /// </summary>
        public virtual IEnumerable<string> Invited { get; set; } = new List<string>();

        /// <summary>
        /// Start date
        /// </summary>
        public virtual DateTime StartDate { get; set; }

        /// <summary>
        /// End date
        /// </summary>
        public virtual DateTime EndDate { get; set; }

        /// <summary>
        /// Url
        /// </summary>
        public  virtual  string BaseAppUrl { get; set; }
    }
}
