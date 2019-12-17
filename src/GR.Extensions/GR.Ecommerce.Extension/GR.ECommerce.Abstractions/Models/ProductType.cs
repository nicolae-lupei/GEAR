using System.ComponentModel.DataAnnotations;
using GR.Core;
using GR.Core.Attributes;

namespace GR.ECommerce.Abstractions.Models
{
    public class ProductType : BaseModel
    {
        [Required]
        [DisplayTranslate(Key = "name")]
        public virtual string Name { get; set; }
        [Required]
        [DisplayTranslate(Key = "display_name")]
        public virtual string DisplayName { get; set; }
    }
}
