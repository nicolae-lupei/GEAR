﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ST.Core.Helpers;
using ST.Identity.Abstractions.Configurations;

namespace ST.Identity.Abstractions.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add context and identity
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="hostingEnvironment"></param>
        /// <param name="migrationsAssembly"></param>
        /// <param name="environment"></param>
        /// <returns></returns>
        public static IServiceCollection AddIdentityModule<TContext>(this IServiceCollection services,
            IConfiguration configuration, IHostingEnvironment hostingEnvironment, string migrationsAssembly,
            IHostingEnvironment environment)
            where TContext : DbContext, IIdentityContext
        {
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<TContext>()
                .AddDefaultTokenProviders();
            return services;
        }

        /// <summary>
        /// Add identity storage
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="migrationsAssembly"></param>
        /// <returns></returns>
        public static IServiceCollection AddIdentityModuleStorage<TIdentityContext>(this IServiceCollection services,
            IConfiguration configuration, string migrationsAssembly)
            where TIdentityContext : DbContext, IIdentityContext
        {
            services.AddDbContext<TIdentityContext>(options =>
            {
                var connectionString = DbUtil.GetConnectionString(configuration);
                if (connectionString.Item1 == DbProviderType.PostgreSql)
                {
                    options.UseNpgsql(connectionString.Item2, opts =>
                    {
                        opts.MigrationsAssembly(migrationsAssembly);
                        opts.MigrationsHistoryTable("IdentityMigrationHistory", IdentityConfig.DEFAULT_SCHEMA);
                    });
                }
                else
                {
                    options.UseSqlServer(connectionString.Item2, opts =>
                    {
                        opts.MigrationsAssembly(migrationsAssembly);
                        opts.MigrationsHistoryTable("IdentityMigrationHistory", IdentityConfig.DEFAULT_SCHEMA);
                    });
                }
            });

            return services;
        }

        /// <summary>
        /// Add provider
        /// </summary>
        /// <typeparam name="TAppProvider"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddAppProvider<TAppProvider>(this IServiceCollection services)
            where TAppProvider : class, IAppProvider
        {
            services.AddTransient<IAppProvider, TAppProvider>();
            IoC.RegisterService<IAppProvider, TAppProvider>();
            return services;
        }
    }
}