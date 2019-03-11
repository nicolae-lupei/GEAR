using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ST.BaseBusinessRepository;
using ST.Entities.Data;
using ST.Entities.Models.Tables;
using ST.Entities.Services;
using ST.Entities.Services.Abstraction;
using ST.Entities.ViewModels.Table;

namespace ST.CORE.Installation
{
	public class EntitySynchronizer
	{
		private readonly string _connectionString;
		private readonly EntitiesDbContext _context;
		private readonly IBaseBusinessRepository<EntitiesDbContext> _repository;

		public EntitySynchronizer(IBaseBusinessRepository<EntitiesDbContext> repository, IConfiguration configuration,
			EntitiesDbContext context)
		{
			Configuration = configuration;
			_repository = repository;
			_context = context;
			_connectionString = _context.Database.GetDbConnection().ConnectionString;
		}

		private static IConfiguration Configuration { get; set; }


		/// <summary>
		/// Sync entities
		/// </summary>
		/// <param name="tableModel"></param>
		/// <param name="schema"></param>
		public void SynchronizeEntities(SynchronizeTableViewModel tableModel, string schema = null)
		{
			var table = new TableModel
			{
				Name = tableModel.Name,
				EntityType = schema ?? tableModel.Schema,
				Description = tableModel.Description,
				IsSystem = tableModel.IsSystem,
				IsPartOfDbContext = tableModel.IsStaticFromEntityFramework
			};
			try
			{
				_context.Table.Add(table);
				_context.SaveChanges();
				CompleteSyncEntity(tableModel, table);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
			}
		}

		/// <summary>
		/// Complete sync entity
		/// </summary>
		/// <param name="tableModel"></param>
		/// <param name="table"></param>
		private void CompleteSyncEntity(SynchronizeTableViewModel tableModel, TableModel table)
		{
			var resultModel = _repository
					.GetAllIncluding<TableModel>(x => x.Include(s => s.TableFields), x => x.Id == table.Id).AsNoTracking()
					.FirstOrDefault();
			if (resultModel == null) return;
			{
				if (tableModel.IsStaticFromEntityFramework)
				{
					var fieldTypeList = _context.TableFieldTypes.ToList();
					var fieldConfigList = _context.TableFieldConfigs.ToList();
					foreach (var item in tableModel.Fields)
					{
						if (item.Configurations != null)
							foreach (var configViewModel in item.Configurations)
							{
								configViewModel.Name = fieldConfigList.Single(x => x.Code == configViewModel.ConfigCode).Name;
							}
						// Save field model in the dataBase
						var configValues = new List<TableFieldConfigValues>();
						var fieldTypeId = fieldTypeList.Single(x => x.Code == item.TableFieldCode).Id;
						var model = new TableModelFields
						{
							DataType = item.DataType,
							TableId = resultModel.Id,
							Description = item.Description,
							Name = item.Name,
							AllowNull = item.AllowNull,
							Synchronized = true,
							TableFieldTypeId = fieldTypeId,
						};
						if (item.Configurations != null)
							foreach (var configItem in item.Configurations)
							{
								var configId = fieldConfigList.Single(x => x.Code == configItem.ConfigCode).Id;
								configValues.Add(new TableFieldConfigValues
								{
									TableFieldConfigId = configId,
									TableModelFieldId = model.Id,
									Value = configItem.Value,
								});
							}
						model.TableFieldConfigValues = configValues;
						_context.TableFields.Add(model);
						_context.SaveChanges();
					}
				}
				else
				{
					ITablesService sqlService = _context.Database.IsNpgsql()
						? new NpgTablesService(repository: _repository) : _context.Database.IsSqlServer()
							? new TablesService(repository: _repository) : null;

					if (sqlService == null) return;
					var response = sqlService.CreateSqlTable(table: resultModel, connectionString: _connectionString);
					if (!response.Result) return;
					// Add
					var fieldTypeList = _context.TableFieldTypes.ToList();
					var fieldConfigList = _context.TableFieldConfigs.ToList();
					//
					foreach (var item in tableModel.Fields)
					{
						foreach (var configViewModel in item.Configurations)
						{
							configViewModel.Name = fieldConfigList.Single(x => x.Code == configViewModel.ConfigCode).Name;

							if (configViewModel.ConfigCode == "9999")
							{
								if (configViewModel.Value == "systemcore")
								{
									configViewModel.Value = table.EntityType;
								}
							}
						}

						var insertField = sqlService.AddFieldSql(item, tableModel.Name, _connectionString, true, table.EntityType);
						// Save field model in the dataBase
						if (!insertField.Result) continue;
						{
							var configValues = new List<TableFieldConfigValues>();
							var fieldTypeId = fieldTypeList.Single(x => x.Code == item.TableFieldCode).Id;
							var model = new TableModelFields
							{
								DataType = item.DataType,
								TableId = resultModel.Id,
								Description = item.Description,
								Name = item.Name,
								AllowNull = item.AllowNull,
								Synchronized = true,
								TableFieldTypeId = fieldTypeId,
							};
							foreach (var configItem in item.Configurations)
							{
								var configId = fieldConfigList.Single(x => x.Code == configItem.ConfigCode).Id;
								configValues.Add(new TableFieldConfigValues
								{
									TableFieldConfigId = configId,
									TableModelFieldId = model.Id,
									Value = configItem.Value,
								});
							}
							model.TableFieldConfigValues = configValues;
							_context.TableFields.Add(model);
							_context.SaveChanges();
						}
					}
				}
			}
		}
	}
}