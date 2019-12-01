﻿using System;
using System.Threading.Tasks;
using GR.Core.Helpers;
using GR.Documents.Abstractions;
using GR.Documents.Abstractions.Models;
using GR.Documents.Abstractions.ViewModels.DocumentViewModels;
using GR.Files.Abstraction;
using GR.Identity.Abstractions;
using GR.WorkFlows.Abstractions;

namespace GR.Documents
{
    public class DocumentWithWorkflowService : DocumentService
    {
        #region Injectable

        /// <summary>
        /// Inject workflow executor
        /// </summary>
        protected readonly IWorkFlowExecutorService WorkFlowExecutorService;

        #endregion

        public DocumentWithWorkflowService(IDocumentContext documentContext, IUserManager<ApplicationUser> userManager, IFileManager fileManager, IWorkFlowExecutorService workFlowExecutorService) : base(documentContext, userManager, fileManager)
        {
            WorkFlowExecutorService = workFlowExecutorService;
        }

        /// <summary>
        /// Add document
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override async Task<ResultModel> AddDocumentAsync(AddDocumentViewModel model)
        {
            var addNewVersionRequest = await base.AddDocumentAsync(model);
            if (!addNewVersionRequest.IsSuccess) return addNewVersionRequest;
            var entryId = (Guid)addNewVersionRequest.Result;
            //TODO: Discuss with Ion if is the current implementation retrieve document version
            return await WorkFlowExecutorService.SetStartStateForEntryAsync(nameof(DocumentVersion), entryId.ToString());
        }

        /// <summary>
        /// Add new document version
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override async Task<ResultModel> AddNewDocumentVersionAsync(AddNewVersionDocumentViewModel model)
        {
            var addNewVersionRequest = await base.AddNewDocumentVersionAsync(model);
            if (!addNewVersionRequest.IsSuccess) return addNewVersionRequest;
            var entryId = (Guid)addNewVersionRequest.Result;
            return await WorkFlowExecutorService.SetStartStateForEntryAsync(nameof(DocumentVersion), entryId.ToString());
        }
    }
}