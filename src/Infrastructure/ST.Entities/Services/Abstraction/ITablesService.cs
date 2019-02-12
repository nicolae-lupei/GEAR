﻿using ST.BaseBusinessRepository;
using ST.Entities.Models.Tables;
using ST.Entities.ViewModels.Table;

namespace ST.Entities.Services.Abstraction
{
    public interface ITablesService
    {
        /// <summary>
        /// Create SQL Table,based on <see cref="TableModel"/>
        /// </summary>
        /// <param name="table"></param>
        /// <param name="connectionString"></param>
        /// <returns><see cref="ResultModel"/></returns>
        /// Type of <see langword="bool"/>
        ResultModel<bool> CreateSqlTable(TableModel table, string connectionString);

        /// <summary>
        /// Add field on table with schema
        /// </summary>
        /// <param name="table"></param>
        /// <param name="tableName"></param>
        /// <param name="connectionString"></param>
        /// <param name="isNew"></param>
        /// <param name="tableSchema"></param>
        /// <returns></returns>
        ResultModel<bool> AddFieldSql(CreateTableFieldViewModel table, string tableName, string connectionString,
            bool isNew, string tableSchema);

        /// <summary>
        /// Check if a specific column has values in dataBase
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        /// <returns><see cref="ResultModel"/></returns>
        /// Type of <see langword="bool"/>
        ResultModel<bool> CheckColumnValues(string connectionString, string tableName, string tableSchema, string columnName);

        /// <summary>
        /// Drop Column from a Table
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        /// <returns><see cref="ResultModel"/></returns>
        /// Type of <see langword="bool"/>
        ResultModel<bool> DropColumn(string connectionString, string tableName, string tableSchema, string columnName);

        /// <summary>
        /// Drop Constraint from a Table
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="tableName"></param>
        /// <param name="constraint"></param>
        /// <param name="columnName"></param>
        /// <returns><see cref="ResultModel"/></returns>
        /// Type of <see langword="bool"/>
        ResultModel<bool> DropConstraint(string connectionString, string tableName, string tableSchema, string constraint,
            string columnName);

        /// <summary>
        /// Check if table has values.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="tableName"></param>
        /// <returns><see cref="ResultModel"/></returns>
        /// Type of <see langword="bool"/>
        ResultModel<bool> CheckTableValues(string connectionString, string tableName, string tableSchema);

        /// <summary>
        /// Drop SQL Table
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="tableName"></param>
        /// <returns><see cref="ResultModel"/></returns>
        /// Type of <see langword="bool"/>
        ResultModel<bool> DropTable(string connectionString, string tableName, string tableSchema);
    }
}