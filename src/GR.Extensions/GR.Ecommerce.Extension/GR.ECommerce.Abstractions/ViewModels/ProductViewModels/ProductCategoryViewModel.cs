using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GR.Core.Attributes;
using GR.ECommerce.Abstractions.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GR.ECommerce.Abstractions.ViewModels.ProductViewModels
{
    public class ProductCategoryViewModel : Category
    {
        /// <summary>
        /// Category list
        /// </summary>
        public List<SelectListItem> ParentCategoryList { get; set; } = new List<SelectListItem>();

        [DisplayTranslate(Key = "iso_active_parent_category")]
        public override Guid? ParentCategoryId { get; set; }

        
        [DisplayTranslate(Key = "display_order")]
        public override int DisplayOrder { get; set; }

        public string CategoryParentName { get; set; }
    }
}
