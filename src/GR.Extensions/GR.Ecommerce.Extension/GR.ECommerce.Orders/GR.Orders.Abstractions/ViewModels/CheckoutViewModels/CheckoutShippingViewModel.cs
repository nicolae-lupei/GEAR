﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GR.Identity.Profile.Abstractions.Models.AddressModels;
using GR.Orders.Abstractions.Models;

namespace GR.Orders.Abstractions.ViewModels.CheckoutViewModels
{
    public class CheckoutShippingViewModel
    {
        /// <summary>
        /// Order
        /// </summary>
        public virtual Order Order { get; set; }

        /// <summary>
        /// Addresses
        /// </summary>
        public virtual IEnumerable<Address> Addresses { get; set; } = new List<Address>();

        /// <summary>
        /// Billing address
        /// </summary>
        [Required]
        public virtual Guid BillingAddressId { get; set; }

        /// <summary>
        /// Shipment address
        /// </summary>
        public virtual Guid ShipmentAddress { get; set; }
    }
}
