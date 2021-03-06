﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using GR.Audit.Abstractions.Extensions;
using GR.Core;
using GR.Core.Events;
using GR.Core.Extensions;
using GR.Core.Helpers;
using GR.Files.Abstraction.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;

namespace GR.Files.Abstraction.Extension
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFileModule<TFileService>(this IServiceCollection services)
            where TFileService : class, IFileManager
        {
            services.AddTransient<IFileManager, TFileService>();
            IoC.RegisterTransientService<IFileManager, TFileService>();
            return services;
        }

        /// <summary>
        /// Add file module storage
        /// </summary>
        /// <typeparam name="TFileContext"></typeparam>
        /// <param name="services"></param>
        /// <param name="options"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddFileModuleStorage<TFileContext>(this IServiceCollection services, Action<DbContextOptionsBuilder> options, IConfiguration configuration)
            where TFileContext : DbContext, IFileContext
        {
            services.AddScopedContextFactory<IFileContext, TFileContext>();
            services.AddDbContext<TFileContext>(options, ServiceLifetime.Transient);
            services.RegisterAuditFor<TFileContext>("Physic File module");
            services.ConfigureWritable<List<FileSettingsViewModel>>(configuration.GetSection("FileSettings"));
            SystemEvents.Database.OnAllMigrate += (sender, args) =>
            {
                GearApplication.GetHost<IWebHost>().MigrateDbContext<TFileContext>();
            };
            return services;
        }
    }
}