﻿using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GR.Audit.Contexts;
using GR.Calendar.Abstractions;
using GR.Calendar.Abstractions.Models;

namespace GR.Calendar.Data
{
    public class CalendarDbContext : TrackerDbContext, ICalendarDbContext
    {
        /// <summary>
        /// Schema
        /// Do not remove this, is used on audit 
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        public const string Schema = "Calendar";

        /// <inheritdoc />
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="options"></param>
        public CalendarDbContext(DbContextOptions<CalendarDbContext> options) : base(options)
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Events
        /// </summary>
        public virtual DbSet<CalendarEvent> CalendarEvents { get; set; }

        /// <inheritdoc />
        /// <summary>
        /// Events
        /// </summary>
        public virtual DbSet<EventMember> EventMembers { get; set; }

        /// <inheritdoc />
        /// <summary>
        /// Provider preferences
        /// </summary>
        public virtual DbSet<UserProviderSyncPreference> UserProviderSyncPreferences { get; set; }

        /// <inheritdoc />
        /// <summary>
        /// External provider tokens
        /// </summary>
        public virtual DbSet<ExternalProviderToken> ExternalProviderTokens { get; set; }

        /// <summary>
        /// Event attributes
        /// </summary>
        public virtual DbSet<EventAttribute> Attributes { get; set; }

        /// <inheritdoc />
        /// <summary>
        /// On model creating
        /// </summary>
        /// <param name="builder"></param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasDefaultSchema(Schema);

            builder.Entity<CalendarEvent>()
                .HasMany(x => x.EventMembers)
                .WithOne(x => x.Event)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<EventMember>().HasKey(x => new { x.EventId, x.UserId });
            builder.Entity<CalendarEvent>().HasIndex(x => new { x.StartDate, x.EndDate });
            builder.Entity<CalendarEvent>().HasIndex(x => new { Owner = x.Organizer });

            builder.Entity<UserProviderSyncPreference>().HasKey(x => new { x.UserId, x.Provider });
            builder.Entity<ExternalProviderToken>().HasKey(x => new { x.UserId, x.Attribute, x.ProviderName });
            builder.Entity<EventAttribute>().HasKey(x => new { x.EventId, x.AttributeName });
        }

        /// <summary>
        /// Seed data
        /// </summary>
        /// <returns></returns>
        public override Task InvokeSeedAsync(IServiceProvider services) => Task.CompletedTask;
    }
}