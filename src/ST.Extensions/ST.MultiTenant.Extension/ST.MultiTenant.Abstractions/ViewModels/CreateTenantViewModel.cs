﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using ST.Core.Attributes;
using ST.Identity.Abstractions.Models.MultiTenants;
using ST.MultiTenant.Abstractions.Helpers;

namespace ST.MultiTenant.Abstractions.ViewModels
{
    public class CreateTenantViewModel : Tenant
    {
        public CreateTenantViewModel()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tenant"></param>
        public CreateTenantViewModel(Tenant tenant)
        {
            CountryId = tenant.CountryId;
            Country = tenant.Country;
            Address = tenant.Address;
            Author = tenant.Author;
            Changed = tenant.Changed;
            Created = tenant.Created;
            CityId = tenant.CityId;
            City = tenant.City;
            DateFormat = tenant.DateFormat;
            Description = tenant.Description;
            Id = tenant.Id;
            IsDeleted = tenant.IsDeleted;
            TimeZone = tenant.TimeZone;
            Name = tenant.Name;
            MachineName = tenant.MachineName;
            SiteWeb = tenant.SiteWeb;
            ModifiedBy = tenant.ModifiedBy;
        }

        /// <summary>
        /// Get base
        /// </summary>
        /// <returns></returns>
        public Tenant GetBase() => new Tenant
        {
            Name = Name,
            CountryId = CountryId,
            OrganizationLogo = OrganizationLogo,
            Description = Description,
            Address = Address,
            MachineName = MachineName,
            Author = Author,
            City = City,
            TimeZone = TimeZone,
            SiteWeb = SiteWeb,
            CityId = CityId,
            DateFormat = DateFormat,
            Country = Country,
            IsDeleted = IsDeleted,
            Id = Id,
            ModifiedBy = ModifiedBy,
            Changed = Changed,
            Created = Created,
            TenantId = TenantId,
            Version = Version
        };
        /// <summary>
        /// Country list
        /// </summary>
        public virtual IEnumerable<SelectListItem> CountrySelectListItems { get; set; } = new List<SelectListItem>();


        /// <summary>
        /// Organization logo
        /// </summary>
        [Display(Name = "Organization Logo")]
        [DisplayTranslate(Key = Resources.Translations.TENANT_LOGO)]
        public virtual IFormFile OrganizationLogoFormFile { get; set; }

        /// <summary>
        /// Machine name
        /// </summary>
        public override string MachineName { get; set; } = $"schema_{Guid.NewGuid().ToString()}";
    }


    public sealed class EditTenantViewModel : CreateTenantViewModel
    {
        public EditTenantViewModel()
        {

        }

        public EditTenantViewModel(Tenant tenant) : base(tenant)
        {

        }
    }
}