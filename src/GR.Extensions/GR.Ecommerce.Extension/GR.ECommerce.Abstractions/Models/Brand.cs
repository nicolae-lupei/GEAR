using System;
using System.ComponentModel.DataAnnotations;
using GR.Core;
using GR.Core.Attributes;

namespace GR.ECommerce.Abstractions.Models
{
    public class Brand : BaseModel
    {
        /// <summary>
        /// Brand name
        /// </summary>
        [Required]
        [DisplayTranslate(Key = "name")]
        public virtual string Name { get; set; }

        /// <summary>
        /// Display name
        /// </summary>
        [DisplayTranslate(Key = "display_name")]
        public virtual string DisplayName { get; set; }
    }
}
