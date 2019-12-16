using System.ComponentModel.DataAnnotations;
using GR.Core;
using GR.Core.Attributes;

namespace GR.ECommerce.Abstractions.Models
{
    public class AttributeGroup : BaseModel
    {
        /// <summary>
        /// Attribute group name
        /// </summary>
        [Required]
        [DisplayTranslate(Key = "name")]
        public virtual string Name { get; set; }
    }
}
