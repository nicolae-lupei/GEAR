﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ST.BaseBusinessRepository;
using ST.BaseRepository;

namespace ST.Entities.Services.Abstraction
{
    public interface IDynamicEntityCreateService
    {
        /// <summary>
        /// Add new value to entity
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel<Guid>> Add<TEntity>(Dictionary<string, object> model) where TEntity : BaseModel;

        /// <summary>
        /// Add new value to entity
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel<Guid>> AddSystem<TEntity>(TEntity model) where TEntity : BaseModel;

        /// <summary>
        /// Add multiples values to entity
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<Guid>>> AddRange<TEntity>(IEnumerable<Dictionary<string, object>> model) where TEntity : BaseModel;
        /// <summary>
        /// Add Range
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        Task<IEnumerable<ResultModel<(TEntity, Guid)>>> AddRange<TEntity>(IEnumerable<TEntity> data) where TEntity : BaseModel;
    }
}
