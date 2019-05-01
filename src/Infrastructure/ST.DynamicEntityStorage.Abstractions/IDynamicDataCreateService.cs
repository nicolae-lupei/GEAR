﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ST.BaseBusinessRepository;
using ST.Core;

namespace ST.DynamicEntityStorage.Abstractions
{
    public interface IDynamicDataCreateService
    {
        /// <summary>
        /// Add new value to entity
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel<Guid>> Add<TEntity>(Dictionary<string, object> model) where TEntity : ExtendedModel;

        /// <summary>
        /// Add new value to entity
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel<Guid>> AddWithReflection<TEntity>(TEntity model) where TEntity : ExtendedModel;

        /// <summary>
        /// Add multiples values to entity
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<Guid>>> AddRange<TEntity>(IEnumerable<Dictionary<string, object>> model) where TEntity : ExtendedModel;
        /// <summary>
        /// Add Range
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        Task<ResultModel<IList<(TEntity, Guid)>>> AddDataRangeWithReflection<TEntity>(IEnumerable<TEntity> data) where TEntity : ExtendedModel;
    }
}
