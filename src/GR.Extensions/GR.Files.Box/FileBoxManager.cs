﻿using Microsoft.AspNetCore.Http;
using GR.Core.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Mapster;
using Microsoft.AspNetCore.Hosting;
using GR.Core.Abstractions;
using GR.Files.Abstraction;
using GR.Files.Abstraction.Helpers;
using GR.Files.Abstraction.Models.ViewModels;
using GR.Files.Box.Abstraction;
using GR.Files.Box.Abstraction.Models;
using GR.Files.Box.Abstraction.Models.ViewModels;
using GR.Files.Box.Data;
using GR.Files.Box.Models;


namespace GR.Files.Box
{
    public class FileBoxManager<TContext> : FileManagerBase, IFileBoxManager where TContext : FileBoxDbContext, IFileBoxContext
    {
        private readonly IWritableOptions<List<FileBoxSettingsViewModel>> _options;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly TContext _context;
        private const string FileRootPath = "FileBox";

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="hostingEnvironment"></param>
        /// <param name="options"></param>
        public FileBoxManager(TContext context, IHostingEnvironment hostingEnvironment, IWritableOptions<List<FileBoxSettingsViewModel>> options)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _options = options;
        }

        public override ResultModel<Guid> AddFile(UploadFileViewModel dto, Guid tenantId)
        {
            var fileValidation =
                FileValidation.ValidateFile(dto.File, _options.Value.FirstOrDefault(x => x.TenantId == tenantId));

            if (!fileValidation.IsSuccess) return fileValidation;

            if (dto.Id != Guid.Empty) return UpdateFile(dto);


            var encryptedFile = SaveFilePhysical(dto.File);
            if (encryptedFile == null) return ExceptionMessagesEnum.NullIFormFile.ToErrorModel<Guid>();

            var file = new FileBox
            {
                FileExtension = encryptedFile.FileExtension,
                Path = encryptedFile.Path,
                Name = encryptedFile.FileName,
                Size = encryptedFile.Size
            };
            _context.FilesBox.Add(file);
            _context.SaveChanges();
            return new ResultModel<Guid>
            {
                IsSuccess = true,
                Result = file.Id
            };
        }

        public override ResultModel<Guid> DeleteFile(Guid id)
        {
            if (id == Guid.Empty) return ExceptionMessagesEnum.NullParameter.ToErrorModel<Guid>();

            var file = _context.FilesBox.FirstOrDefault(x => x.Id == id);
            if (file == null) return ExceptionMessagesEnum.FileNotFound.ToErrorModel<Guid>();

            file.IsDeleted = true;
            _context.FilesBox.Update(file);
            _context.SaveChanges();

            return new ResultModel<Guid>
            {
                IsSuccess = true
            };
        }

        public override ResultModel<Guid> RestoreFile(Guid id)
        {
            if (id == Guid.Empty) return ExceptionMessagesEnum.NullParameter.ToErrorModel<Guid>();

            var file = _context.FilesBox.FirstOrDefault(x => x.Id == id);
            if (file == null) return ExceptionMessagesEnum.FileNotFound.ToErrorModel<Guid>();

            file.IsDeleted = false;
            _context.FilesBox.Update(file);
            _context.SaveChanges();


            return new ResultModel<Guid>
            {
                IsSuccess = true
            };
        }

        public override ResultModel<Guid> DeleteFilePermanent(Guid id)
        {
            if (id == Guid.Empty) return ExceptionMessagesEnum.NullParameter.ToErrorModel<Guid>();

            var file = _context.FilesBox.FirstOrDefault(x => x.Id == id);
            if (file == null) return ExceptionMessagesEnum.FileNotFound.ToErrorModel<Guid>();

            var filePath = Path.Combine(_hostingEnvironment.WebRootPath, FileRootPath, file.Path, file.Name);
            File.Delete(filePath);
            _context.FilesBox.Remove(file);

            _context.SaveChanges();
            return new ResultModel<Guid>
            {
                IsSuccess = true
            };
        }

        public override ResultModel<DownloadFileViewModel> GetFileById(Guid id)
        {
            if (id == Guid.Empty) return ExceptionMessagesEnum.NullParameter.ToErrorModel<DownloadFileViewModel>();

            var dbFileResult = _context.FilesBox.FirstOrDefault(x => (x.Id == id) & (x.IsDeleted == false));
            if (dbFileResult == null) return ExceptionMessagesEnum.FileNotFound.ToErrorModel<DownloadFileViewModel>();

            var filePath = Path.Combine(_hostingEnvironment.WebRootPath, FileRootPath, dbFileResult.Path);
            var dto = new DownloadFileViewModel
            {
                Path = filePath, FileExtension = dbFileResult.FileExtension, FileName = dbFileResult.Name
            };

            return new ResultModel<DownloadFileViewModel>
            {
                IsSuccess = true,
                Result = dto
            };
        }

        public override ResultModel ChangeSettings<TFileSettingsViewModel>(TFileSettingsViewModel newSettings)
        {
            var settings = newSettings.Adapt<FileBoxSettingsViewModel>();
            var result = new ResultModel();
            var fileSettingsList = _options.Value ?? new List<FileBoxSettingsViewModel>();
            var fileSettings = _options?.Value?.Find(x => x.TenantId == newSettings.TenantId);
            if (fileSettings == null)
            {
                fileSettingsList.Add(settings);
            }
            else
            {
                var index = fileSettingsList.FindIndex(m => m.TenantId == newSettings.TenantId);
                if (index >= 0)
                    fileSettingsList[index] = settings;
            }

            _options.Update(x => x = fileSettingsList, "fileSettings.json");
            result.IsSuccess = true;
            return result;
        }

        private FileBoxDto SaveFilePhysical(IFormFile file)
        {
            if (file.Length <= 0) return null;

            var directory = Path.Combine(_hostingEnvironment.WebRootPath, FileRootPath, DateTime.Now.ToLongDateString());
            var exists = Directory.Exists(directory);
            if (!exists) Directory.CreateDirectory(directory);
            var filePath = Path.Combine(directory, file.FileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            var response = new FileBoxDto
            {
                FileExtension = file.ContentType,
                Path = DateTime.Now.ToLongDateString(),
                FileName = file.FileName,
                Size = file.Length
            };
            return response;
        }

        private ResultModel<Guid> UpdateFile(UploadFileViewModel dto)
        {
            var file = _context.FilesBox.FirstOrDefault(x => x.Id == dto.Id);
            if (file == null)
                return ExceptionMessagesEnum.FileNotFound.ToErrorModel<Guid>();

            DeleteFilePhysical(file.Path, file.Name);
            var encryptedFile = SaveFilePhysical(dto.File);
            if (encryptedFile == null) return ExceptionMessagesEnum.NullIFormFile.ToErrorModel<Guid>();

            file.FileExtension = encryptedFile.FileExtension;
            file.Path = encryptedFile.Path;
            file.Name = encryptedFile.FileName;
            file.Size = encryptedFile.Size;
            _context.FilesBox.Update(file);
            _context.SaveChanges();

            return new ResultModel<Guid>
            {
                IsSuccess = true,
                Result = file.Id
            };
        }

        private void DeleteFilePhysical(string path, string fileName)
        {
            var filePath = Path.Combine(_hostingEnvironment.WebRootPath, FileRootPath, path, fileName);
            File.Delete(filePath);
        }

    }
}
