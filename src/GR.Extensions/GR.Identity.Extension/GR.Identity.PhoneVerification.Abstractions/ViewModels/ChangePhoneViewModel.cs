﻿using System.ComponentModel.DataAnnotations;

namespace GR.Identity.PhoneVerification.Abstractions.ViewModels
{
    public class ChangePhoneViewModel
    {
        /// <summary>
        /// Country code
        /// </summary>
        [Required]
        [StringLength(4, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
        public string CountryCode { get; set; }

        /// <summary>
        /// Phone number
        /// </summary>
        [Required]
        [StringLength(16, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 7)]
        public string PhoneNumber { get; set; }
    }
}