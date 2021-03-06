﻿using System;
using Microsoft.AspNetCore.Http;

namespace GR.Files.Abstraction.Models.ViewModels
{
    public class UploadFileViewModel
    {
        /// <summary>
        /// Hash
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Hash
        /// </summary>
        public IFormFile File { get; set; }
    }
}