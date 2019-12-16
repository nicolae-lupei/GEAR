using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GR.Core.Attributes;
using GR.ECommerce.Abstractions.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GR.ECommerce.Abstractions.ViewModels.ProductViewModels
{
    public class ProductViewModel : Product
    {
        /// <summary>
        /// Brands
        /// </summary>
        public List<SelectListItem> Brands { get; set; } = new List<SelectListItem>();

        /// <summary>
        /// Display name
        /// </summary>
        [DisplayTranslate(Key = "display_name")]
        public override string DisplayName { get; set; }

        /// <summary>
        /// Short description
        /// </summary>
        [DisplayTranslate(Key = "short_description")]
        public override string ShortDescription { get; set; }

        /// <summary>
        /// Attributes
        /// </summary>
        public Dictionary<string, IEnumerable<SelectListItem>> ProductAttributesList { get; set; } = new Dictionary<string, IEnumerable<SelectListItem>>();

        [DisplayTranslate(Key = "available_attributes")]
        public int ProductAttributeId { get; set; }

        /// <summary>
        /// Category list
        /// </summary>
        public List<ProductCategoryDto> ProductCategoryList { get; set; } = new List<ProductCategoryDto>();
        public List<SelectListItem> ProductOption { get; set; } = new List<SelectListItem>();
        public List<ProductVariation> ProductVariationList { get; set; } = new List<ProductVariation>();


        /// <summary>
        /// Images
        /// </summary>
        [DisplayTranslate(Key = "product_image")]
        public List<IFormFile> ProductImagesList { get; set; } = new List<IFormFile>();

        /// <summary>
        /// Product types
        /// </summary>
        [DisplayTranslate(Key = "type")]
        public List<SelectListItem> ProductTypeList { get; set; } = new List<SelectListItem>();

        /// <summary>
        /// Last price
        /// </summary>
         [DisplayTranslate(Key = "price")]
        public virtual decimal Price { get; set; }
    }

    public class ProductCategoryDto
    {
        public Guid CategoryId { get; set; }
        public string Name { get; set; }
        public bool IsChecked { get; set; }
    }
}
