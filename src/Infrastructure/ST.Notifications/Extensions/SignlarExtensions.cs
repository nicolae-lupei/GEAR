﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using ST.Notifications.Abstraction;
using ST.Notifications.Hubs;
using ST.Notifications.Services;

namespace ST.Notifications.Extensions
{
    public static class SignlarExtensions
    {
        /// <summary>
        /// Use signalR from ST.Notifications By Soft-Tehnica
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseStSignalR(this IApplicationBuilder app)
        {
            app.UseSignalR(routes =>
            {
                routes.MapHub<NotificationsHub>("/rtn",
                    options =>
                    {
                        options.Transports = Microsoft.AspNetCore.Http.Connections.HttpTransports.All;
                    });
            });
            return app;
        }
        /// <summary>
        /// Add SignalR from ST.Notifications By Soft-Tehnica
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddStSignalR(this IServiceCollection services)
        {
            services.AddSignalR(options =>
            {
                options.EnableDetailedErrors = true;
            });
            services.AddSingleton<IUserIdProvider, NameUserIdProvider>();
            services.AddTransient<INotificationHub, NotificationProvider>();
            services.AddTransient<INotify, Notify>();
            return services;
        } 
    }
}
