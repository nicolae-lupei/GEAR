﻿using Microsoft.EntityFrameworkCore.Design;
using ST.Core.Helpers.DbContexts;

namespace ST.Entities.Security.Data
{
    /// <summary>
    /// Do not remove this
    /// It is used for generate migrations
    /// </summary>
    public class EntitySecurityDbContextFactory : IDesignTimeDbContextFactory<EntitySecurityDbContext>
    {
        /// <inheritdoc />
        /// <summary>
        /// For creating migrations
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public EntitySecurityDbContext CreateDbContext(string[] args)
        {
            return DbContextFactory<EntitySecurityDbContext, EntitySecurityDbContext>.CreateFactoryDbContext();
        }
    }
}
