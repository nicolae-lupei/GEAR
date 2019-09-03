﻿using System.ComponentModel.DataAnnotations;
using ST.Audit.Abstractions.Attributes;
using ST.Audit.Abstractions.Enums;
using ST.Core.Abstractions;

namespace ST.Identity.Abstractions.Models.AddressModels
{
    [TrackEntity(Option = TrackEntityOption.AllFields)]
    public class StateOrProvince : IBase<long>
    {
        public long Id { get; set; }

        public StateOrProvince()
        {

        }

        public StateOrProvince(long id)
        {
            Id = id;
        }

        [StringLength(450)]
        public string CountryId { get; set; }

        public Country Country { get; set; }

        [StringLength(450)]
        public string Code { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [StringLength(450)]
        public string Name { get; set; }

        [StringLength(450)]
        public string Type { get; set; }
    }
}
