﻿using System;
using System.Threading.Tasks;
using GR.ECommerce.Payments.Abstractions;
using GR.Paypal.Abstractions.ViewModels;

namespace GR.Paypal.Abstractions
{
    public interface IPaypalPaymentMethodService : IPaymentMethodService
    {
        Task<ResponsePaypal> CreatePayment(string hostingDomain, Guid? orderId);
        Task<ResponsePaypal> ExecutePayment(PaymentExecuteVm model);
    }
}