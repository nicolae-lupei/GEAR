using System;
using System.ComponentModel.DataAnnotations;
using GR.Core;
using GR.Core.Attributes;

namespace GR.ECommerce.Abstractions.Models
{
    public class ProductAttribute : BaseModel
    {
        /// <summary>
        /// Attribute name
        /// </summary>
        [Required]
        [DisplayTranslate(Key = "name")]
        public virtual string Name { get; set; }
        /// <summary>
        /// Display name
        /// </summary>
        [Required]
        [DisplayTranslate(Key = "display_name")]
        public virtual string DisplayName { get; set; }

        /// <summary>
        /// Reference to group
        /// </summary>
        public virtual AttributeGroup AttributeGroup { get; set; }
        public virtual Guid? AttributeGroupId { get; set; }
    }
}
