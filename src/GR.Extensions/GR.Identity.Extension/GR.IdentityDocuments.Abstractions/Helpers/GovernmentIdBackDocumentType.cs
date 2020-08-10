﻿using System.Collections.Generic;
using GR.IdentityDocuments.Abstractions.Models;
using Microsoft.Extensions.Localization;

namespace GR.IdentityDocuments.Abstractions.Helpers
{
    public class GovernmentIdBackDocumentType : IdentityDocumentType
    {
        #region Injectable

        private readonly IStringLocalizer _localizer;

        #endregion

        public GovernmentIdBackDocumentType(IStringLocalizer localizer)
        {
            _localizer = localizer;
            SetUp();
        }

        public void SetUp()
        {
            Id = "governmentIdBack";
            Name = "Personal ID Back";
            Description = "Upload your identification document back in clear view, or upload passport.";
            Requirements = new List<string>
            {
                "Max size 10MB",
                "Allowed file extensions: JPG, JPEG, BMP, PNG"
            };
        }
    }
}