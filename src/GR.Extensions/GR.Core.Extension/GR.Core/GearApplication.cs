﻿using System;
using System.Runtime.InteropServices;
using Castle.Windsor;
using GR.Core.Helpers;

namespace GR.Core
{
    public abstract class GearApplication
    {
        /// <summary>
        /// Services container
        /// </summary>
        public static IWindsorContainer ServicesContainer => IoC.Container;

        /// <summary>
        /// Is configured
        /// </summary>
        protected static bool ConfigurationsApplied { get; set; }

        /// <summary>
        /// Is configured
        /// </summary>
        public static bool Configured => ConfigurationsApplied;

        /// <summary>
        /// App host
        /// </summary>
        protected static dynamic GlobalAppHost { get; set; }

        /// <summary>
        /// Configuration
        /// </summary>
        protected static dynamic GlobalAppConfiguration { get; set; }

        /// <summary>
        /// Get app host
        /// </summary>
        /// <typeparam name="THost"></typeparam>
        /// <returns></returns>
        public static THost GetHost<THost>() => (THost)GlobalAppHost;

        /// <summary>
        /// App configuration
        /// </summary>
        /// <typeparam name="TConfiguration"></typeparam>
        /// <returns></returns>
        public static TConfiguration GetConfiguration<TConfiguration>() => (TConfiguration)GlobalAppConfiguration;

        /// <summary>
        /// Check platform host
        /// </summary>
        /// <returns></returns>
        public static bool IsHostedOnLinux() => RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

        /// <summary>
        /// Running project path
        /// </summary>
        public static string RunningProjectPath => AppContext.BaseDirectory.Substring(0, AppContext.BaseDirectory.IndexOf("bin", StringComparison.Ordinal));
    }
}