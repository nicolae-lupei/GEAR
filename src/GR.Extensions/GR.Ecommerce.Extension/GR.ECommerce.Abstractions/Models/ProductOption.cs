using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using GR.Core;
using GR.Core.Attributes;

namespace GR.ECommerce.Abstractions.Models
{
    public class ProductOption : BaseModel
    {
        
        [Required(AllowEmptyStrings = false)]
        [DisplayTranslate(Key = "name")]
        public virtual string Name { get; set; }
        [Required]
        [DisplayTranslate(Key = "system_is_published")]
        public virtual bool IsPublish { get; set; }
    }
}
