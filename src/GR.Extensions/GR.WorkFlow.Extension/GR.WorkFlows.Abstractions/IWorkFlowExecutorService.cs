﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using GR.Core.Helpers;
using GR.Identity.Abstractions;
using GR.WorkFlows.Abstractions.Models;
using GR.WorkFlows.Abstractions.ViewModels;

namespace GR.WorkFlows.Abstractions
{
    public interface IWorkFlowExecutorService
    {
        /// <summary>
        /// Register entity contract
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="workFlowId"></param>
        /// <returns></returns>
        Task<ResultModel<Guid>> RegisterEntityContractToWorkFlowAsync([Required] string entityName, Guid? workFlowId);

        /// <summary>
        /// Check for registered contract to entity
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="workFlowId"></param>
        /// <returns></returns>
        Task<bool> IsAnyRegisteredContractToEntityAsync([Required] string entityName, Guid? workFlowId);

        /// <summary>
        /// Execute actions
        /// </summary>
        /// <param name="transition"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        Task ExecuteActionsAsync(Transition transition, Dictionary<string, object> data);

        /// <summary>
        /// Force execute transition actions
        /// </summary>
        /// <param name="entryId"></param>
        /// <param name="transitionId"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        Task<ResultModel> ForceExecuteTransitionActionsAsync(Guid? entryId, Guid? transitionId, Dictionary<string, object> data);

        /// <summary>
        /// Get roles for transition
        /// </summary>
        /// <param name="transition"></param>
        /// <returns></returns>
        Task<IEnumerable<ApplicationRole>> GetAllowedRolesToTransitionAsync(Transition transition);

        /// <summary>
        /// Get next possible transitions
        /// </summary>
        /// <param name="transition"></param>
        /// <returns></returns>
        Task<IEnumerable<Transition>> GetNextTransitionsAsync(Transition transition);

        /// <summary>
        /// Get next transitions from state
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<Transition>>> GetNextTransitionsFromStateAsync([Required] State state);

        /// <summary>
        /// Get next states
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<State>>> GetNextStatesAsync([Required] State state);

        /// <summary>
        /// Get entry state
        /// </summary>
        /// <param name="entryId"></param>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<EntryState>>> GetEntryStatesAsync([Required] string entryId);


        /// <summary>
        /// Get entry state
        /// </summary>
        /// <param name="entryId"></param>
        /// <param name="workFlowId"></param>
        /// <returns></returns>
        Task<ResultModel<EntryState>> GetEntryStateAsync([Required] string entryId, [Required] Guid? workFlowId);

        /// <summary>
        /// Get next states for entry
        /// </summary>
        /// <param name="entryId"></param>
        /// <param name="workFlowId"></param>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<StateGetViewModel>>> GetNextStatesForEntryAsync([Required] string entryId, [Required] Guid? workFlowId);

        /// <summary>
        /// Get entity contracts
        /// </summary>
        /// <param name="entityName"></param>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<WorkFlowEntityContract>>> GetEntityContractsAsync([Required] string entityName);

        /// <summary>
        /// Set start state for entry on all registered workflows
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="entryId"></param>
        /// <returns></returns>
        Task<ResultModel> SetStartStateForEntryAsync([Required] string entityName, [Required] string entryId);

        /// <summary>
        /// Get next states from state
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<State>>> GetNextStatesForAllowedRolesAsync([Required] State state);

        /// <summary>
        /// Change state for entry 
        /// </summary>
        /// <param name="entryId"></param>
        /// <param name="workFlowId"></param>
        /// <param name="newStateId"></param>
        /// <returns></returns>
        Task<ResultModel> ChangeStateForEntryAsync([Required]string entryId, [Required] Guid? workFlowId, [Required] Guid? newStateId);
    }
}