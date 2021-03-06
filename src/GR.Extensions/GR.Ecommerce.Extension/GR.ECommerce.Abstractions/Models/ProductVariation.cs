﻿using System;
using System.Collections.Generic;
using GR.Core;

namespace GR.ECommerce.Abstractions.Models
{
     public class ProductVariation: BaseModel
    {
        public virtual Product Product { get; set; }
        public virtual Guid ProductId { get; set; }

        public virtual decimal Price { get; set; } = 0;

        public virtual List<ProductVariationDetail> ProductVariationDetails { get; set; }

    }
}
