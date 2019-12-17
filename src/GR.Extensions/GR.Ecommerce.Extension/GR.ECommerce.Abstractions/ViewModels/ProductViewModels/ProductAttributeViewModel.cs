using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GR.Core.Attributes;
using GR.ECommerce.Abstractions.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GR.ECommerce.Abstractions.ViewModels.ProductViewModels
{
    public class ProductAttributeViewModel : ProductAttribute
    {
        /// <summary>
        /// Groups
        /// </summary>
        public List<SelectListItem> AttributeGroups { get; set; } = new List<SelectListItem>();

        /// <summary>
        /// Group
        /// </summary>
        [DisplayTranslate(Key = "attribute_group_name")]
        
        public override Guid? AttributeGroupId { get; set; }
    }
}
