﻿using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace GR.Core.Helpers
{
    public class ResultModel<T> : IResultModel<T>
    {
        /// <inheritdoc />
        /// <summary>
        /// Bool indicating that the request resulted with success.
        /// If False than the <see cref="T:GR.Core.Helpers.ErrorModel" /> will 
        /// contain a Error message that produced this error.
        /// </summary>
        [JsonProperty("is_success")]
        public virtual bool IsSuccess { get; set; }

        /// <inheritdoc />
        /// <summary>
        /// This property will contain error keys if any.
        /// </summary>
        [JsonProperty("error_keys")]
        public virtual ICollection<IErrorModel> Errors { get; set; } = new List<IErrorModel>();

        /// <inheritdoc />
        /// <summary>
        /// The result of the response, if there is no errors.
        /// </summary>
        [JsonProperty("result")]
        public virtual T Result { get; set; }

        /// <summary>
        /// To base
        /// </summary>
        /// <returns></returns>
        public virtual ResultModel ToBase() => new ResultModel
        {
            IsSuccess = IsSuccess,
            Result = Result,
            Errors = Errors
        };

        /// <summary>
        /// Add error
        /// </summary>
        /// <param name="error"></param>
        public virtual ResultModel<T> AddError(string error)
        {
            Errors?.Add(new ErrorModel(string.Empty, error));
            return this;
        }

        /// <summary>
        /// Add error
        /// </summary>
        /// <param name="key"></param>
        /// <param name="error"></param>
        public virtual ResultModel<T> AddError(string key, string error)
        {
            Errors?.Add(new ErrorModel(key, error));
            return this;
        }

        /// <summary>
        /// Add error from results
        /// </summary>
        /// <param name="results"></param>
        /// <returns></returns>
        public virtual ResultModel<T> JoinResults<TIn>(IEnumerable<ResultModel<TIn>> results)
        {
            var collection = results.ToList();
            var response = new ResultModel<T>
            {
                IsSuccess = collection.All(x => x.IsSuccess)
            };
            foreach (var result in collection)
            {
                foreach (var error in result.Errors)
                {
                    response.Errors.Add(error);
                }
            }

            return response;
        }

        /// <summary>
        /// Set result
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public virtual ResultModel<T> SetResult(T result)
        {
            Result = result;
            return this;
        }

        /// <summary>
        /// Has error code
        /// </summary>
        /// <param name="errorCode"></param>
        /// <returns></returns>
        public virtual bool HasErrorCode(string errorCode)
        {
            return Errors?.Any(x => x.Key.Equals(errorCode)) ?? false;
        }

        /// <summary>
        /// Adapt
        /// </summary>
        /// <typeparam name="TModelOutput"></typeparam>
        /// <param name="result"></param>
        /// <returns></returns>
        public virtual ResultModel<TModelOutput> Map<TModelOutput>(TModelOutput result = default) => new ResultModel<TModelOutput>
        {
            IsSuccess = IsSuccess,
            Result = result,
            Errors = Errors
        };

        private static ResultModel<T> _instance;
        public static ResultModel<T> Instance => _instance ?? (_instance = new ResultModel<T>());
    }
}