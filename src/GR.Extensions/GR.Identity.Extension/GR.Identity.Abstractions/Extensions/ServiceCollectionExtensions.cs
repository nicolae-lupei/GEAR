﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using GR.Audit.Abstractions.Extensions;
using GR.Core;
using GR.Core.Extensions;
using GR.Core.Helpers;
using GR.Core.Helpers.Scopes;
using GR.Identity.Abstractions.Configurations;
using GR.Identity.Abstractions.Events;
using GR.Identity.Abstractions.Helpers;
using GR.Identity.Abstractions.Helpers.PasswordPolicies;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GR.Identity.Abstractions.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add context and identity
        /// </summary>
        /// <param name="services"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IdentityBuilder AddIdentityModule<TContext>(this IServiceCollection services, Action<IdentityOptions> options = null)
            where TContext : DbContext, IIdentityContext
        {
            var builder = services.AddIdentity<GearUser, GearRole>(o =>
                {
                    o.Lockout = new LockoutOptions
                    {
                        MaxFailedAccessAttempts = 4,
                        AllowedForNewUsers = true,
                        DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15)
                    };
                    options?.Invoke(o);
                })
                .AddEntityFrameworkStores<TContext>()
                .AddDefaultTokenProviders();

            return builder;
        }

        /// <summary>
        /// Set password policy
        /// </summary>
        /// <param name="services"></param>
        /// <param name="passwordOptions"></param>
        /// <returns></returns>
        public static IServiceCollection PasswordPolicy(this IServiceCollection services, Action<PasswordOptions> passwordOptions)
        {
            // Configure Identity
            services.Configure<IdentityOptions>(options =>
            {
                passwordOptions(options.Password);
            });

            return services;
        }

        /// <summary>
        /// Password policy
        /// </summary>
        /// <typeparam name="TPasswordPolicy"></typeparam>
        /// <param name="services"></param>
        /// <param name="passwordPolicy"></param>
        /// <returns></returns>
        public static IServiceCollection PasswordPolicy<TPasswordPolicy>(this IServiceCollection services, TPasswordPolicy passwordPolicy = null)
            where TPasswordPolicy : PasswordPolicy
        {
            if (passwordPolicy == null)
                passwordPolicy = new DefaultPasswordPolicy() as TPasswordPolicy;
            // Configure Identity
            services.Configure<IdentityOptions>(options =>
            {
                options.Password = passwordPolicy?.Policy(options.Password);
            });

            return services;
        }

        /// <summary>
        /// Add authentication
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config"></param>
        /// <param name="useHttps"></param>
        /// <returns></returns>
        public static IServiceCollection AddAuthentication<TAuthorizeService>(this IServiceCollection services, Action<GearAuthenticationOptions> config = null,  bool useHttps = false)
            where TAuthorizeService : class, IAuthorizeService
        {
            services.AddGearTransient<IAuthorizeService, TAuthorizeService>();
            var authorityUri = GearApplication.SystemConfig.EntryUri.ToString();

            var hostingEnvironment = services
                .BuildServiceProvider()
                .GetRequiredService<IHostingEnvironment>();
            var isDevelopment = hostingEnvironment.IsDevelopment() || !useHttps;

            Action<CookieAuthenticationOptions> cookieAuthOptions = options =>
            {
                options.Cookie.Name = $".AspNet{GearApplication.SystemConfig.MachineIdentifier}";
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = isDevelopment
                    ? CookieSecurePolicy.None
                    : CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.Lax;
                options.Cookie.IsEssential = true;
                options.EventsType = typeof(CustomCookieAuthenticationEvents);
            };

            services.AddAuthentication(options =>
                {
                    //options.DefaultScheme = "gear";
                    //options.DefaultAuthenticateScheme = "gear";
                })
                .AddPolicyScheme("gear", "Authorization Bearer or Identity.Application", options =>
                {
                    options.ForwardDefaultSelector = context =>
                    {
                        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
                        if (authHeader?.StartsWith("Bearer ") ?? false)
                        {
                            return JwtBearerDefaults.AuthenticationScheme;
                        }

                        return IdentityConstants.ApplicationScheme;
                    };
                })
                .AddJwtBearer("Bearer", opts =>
                {
                    opts.Audience = GearScopes.CORE;
                    opts.Authority = authorityUri;
                    opts.RequireHttpsMetadata = false;
                    opts.SaveToken = true;
                    opts.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            Console.WriteLine(context.Token);
                            return Task.CompletedTask;
                        },
                        OnAuthenticationFailed = context =>
                        {
                            Console.WriteLine(context.Exception);
                            return Task.CompletedTask;
                        },
                        OnChallenge = context => Task.CompletedTask,
                        OnTokenValidated = context => Task.CompletedTask
                    };
                })
                .AddCookie(options =>
                {
                    cookieAuthOptions.Invoke(options);
                });

            services.AddScoped<CustomCookieAuthenticationEvents>();

            services.ConfigureApplicationCookie(options =>
            {
                cookieAuthOptions.Invoke(options);
            });

            var baseConf = new GearAuthenticationOptions();
            config?.Invoke(baseConf);
            services.AddGearSingleton(baseConf);
            return services;
        }

        /// <summary>
        /// Add identity user manager
        /// </summary>
        /// <typeparam name="TUserManager"></typeparam>
        /// <typeparam name="TUser"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddIdentityUserManager<TUserManager, TUser>(this IServiceCollection services)
            where TUser : GearUser
            where TUserManager : class, IUserManager<TUser>
        {
            services.AddGearScoped<IUserManager<TUser>, TUserManager>();
            //Register user manager
            IoC.Container.Register(Component.For<UserManager<GearUser>>());
            return services;
        }

        /// <summary>
        /// Add identity storage
        /// </summary>
        /// <param name="services"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IServiceCollection AddIdentityModuleStorage<TIdentityContext>(this IServiceCollection services, Action<DbContextOptionsBuilder> options)
            where TIdentityContext : DbContext, IIdentityContext
        {
            services.AddTransient<IIdentityContext, TIdentityContext>();
            services.AddDbContext<TIdentityContext>(options);
            services.RegisterAuditFor<IIdentityContext>("Identity module");
            return services;
        }

        /// <summary>
        /// Register events
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddIdentityModuleEvents(this IServiceCollection services)
        {
            IdentityEvents.RegisterEvents();
            return services;
        }
    }
}