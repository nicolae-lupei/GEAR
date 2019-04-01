﻿using Microsoft.Extensions.DependencyInjection;
using ST.DynamicEntityStorage.Abstractions;
using ST.Entities.Data;

namespace ST.DynamicEntityStorage.Extensions
{
    public static class RegisterServiceCollection
    {
        /// <summary>
        /// Register new data access services
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection RegisterDynamicDataServices<TContext>(this IServiceCollection services) where TContext : EntitiesDbContext
        {
            services.AddTransient<IDynamicService, DynamicService<TContext>>();
            services.AddTransient<IDynamicDataGetService, DynamicService<TContext>>();
            services.AddTransient<IDynamicDataCreateService, DynamicService<TContext>>();
            services.AddTransient<IDynamicDataUpdateService, DynamicService<TContext>>();
            return services;
        }
    }
}
