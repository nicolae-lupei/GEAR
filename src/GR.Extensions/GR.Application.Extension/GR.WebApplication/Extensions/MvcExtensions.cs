﻿using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using GR.Core;
using GR.Core.Attributes.Documentation;
using GR.Core.Extensions;
using GR.Core.Helpers;
using GR.Core.Helpers.Global;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;

namespace GR.WebApplication.Extensions
{
    /// <summary>
    /// This class contains MVC builder extensions
    /// </summary>
    [Author(Authors.LUPEI_NICOLAE, 1.1)]
    public static class MvcExtensions
    {
        /// <summary>
        /// Use hot reload for views from razor class libraries
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IMvcBuilder AddGearViewsHotReload(this IMvcBuilder builder)
        {
            if (!Debugger.IsAttached || !GearApplication.IsDevelopment()) return builder;
            ConsoleWriter.WriteTextAsTitle("Hot reload tool", ConsoleColor.DarkMagenta);
            var root = AppContext.BaseDirectory.GetParentDirectory("src");
            var directories = Directory.GetDirectories(root, "*.Razor", SearchOption.AllDirectories);
            if (!directories.Any()) return builder;

            builder.AddRazorRuntimeCompilation();
            var serviceProvider = builder.Services.BuildServiceProvider();
            var logger = serviceProvider.GetService<ILogger<IMvcBuilder>>();
            builder.Services.Configure<MvcRazorRuntimeCompilationOptions>(options =>
            {
                foreach (var directory in directories)
                {
                    var path = Path.Combine(directory, "Views");
                    if (!Directory.Exists(path)) continue;

                    options.FileProviders.Add(new PhysicalFileProvider(directory));

                    logger.LogInformation($"Added {directory} project to detect changes");
                }
            });
            return builder;
        }
    }
}