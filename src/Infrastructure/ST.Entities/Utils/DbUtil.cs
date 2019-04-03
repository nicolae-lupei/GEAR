﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ST.Entities.Services;
using ST.Entities.Services.Abstraction;

namespace ST.Entities.Utils
{
    public static class DbUtil
    {
        /// <summary>
        /// GetConnectionString connection string
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="env"></param>
        /// <returns></returns>
        public static (DbProviderType, string) GetConnectionString(IConfiguration configuration, IHostingEnvironment env)
        {
            DbProviderType dbType;
            var postgreSettings = configuration.GetSection("ConnectionStrings")
                .GetSection("PostgreSQL");
            var isPostgreSql = postgreSettings.GetValue<bool>("UsePostgreSQL");
            string connectionString = "";
            if (isPostgreSql)
            {
                connectionString = postgreSettings
                    .GetValue<string>("ConnectionString");
                dbType = DbProviderType.PostgreSql;
            }
            else
            {
                connectionString = configuration.GetConnectionString("MSSQLConnection");
                dbType = DbProviderType.MsSqlServer;
            }

            return (dbType, connectionString);
        }

        public static string GetConnectionString<TContext>(this TContext context) where TContext : DbContext
        {
            return context.Database.GetDbConnection().ConnectionString;
        }

        public static DbProviderType GetProviderType<TContext>(this TContext context) where TContext : DbContext
        {
            var result = DbProviderType.PostgreSql;
            var provider = context.Database.ProviderName;
            switch (provider)
            {
                case DbProvider.PostgreSQL:
                    result = DbProviderType.PostgreSql;
                    break;
                case DbProvider.SqlServer:
                    result = DbProviderType.MsSqlServer;
                    break;
            }

            return result;
        }

        /// <summary>
        /// GetConnectionString Sql table service
        /// </summary>
        /// <param name="databaseProvider"></param>
        /// <returns></returns>
        public static ITablesService GetSqlTableService(DbProviderType databaseProvider)
        {
            return databaseProvider.Equals(DbProviderType.MsSqlServer)
                ? new TablesService()
                : databaseProvider.Equals(DbProviderType.PostgreSql)
                    ? new NpgTablesService()
                    : null;
        }

    }

    public enum DbProviderType
    {
        MsSqlServer, PostgreSql
    }

    public static class DbProvider
    {
        public const string SqlServer = "Microsoft.EntityFrameworkCore.SqlServer";
        public const string Sqlite = "Microsoft.EntityFrameworkCore.Sqlite";
        public const string InMemory = "Microsoft.EntityFrameworkCore.InMemory";
        public const string Cosmos = "Microsoft.EntityFrameworkCore.Cosmos";
        public const string PostgreSQL = "Npgsql.EntityFrameworkCore.PostgreSQL";
        public const string MySql = "Pomelo.EntityFrameworkCore.MySql";
        public const string MyCat = "Pomelo.EntityFrameworkCore.MyCat";
        public const string SqlServerCompact40 = "EntityFrameworkCore.SqlServerCompact40";
        public const string SqlServerCompact35 = "EntityFrameworkCore.SqlServerCompact35";
        public const string Jet = "EntityFrameworkCore.Jet";
        public const string MySqlData = "MySql.Data.EntityFrameworkCore";
        public const string Firebird = "FirebirdSql.EntityFrameworkCore.Firebird";
        public const string FirebirdSql = "EntityFrameworkCore.FirebirdSql";
        public const string IBM = "IBM.EntityFrameworkCore";
        public const string OpenEdge = "EntityFrameworkCore.OpenEdge";
    }
}
