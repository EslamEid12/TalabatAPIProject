﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Core.Services
{
    public interface IPaymentServices
    {
        Task<CustomerBasket?> CreateOrUpadtePaymentIntent(string basketId);
        Task<Order> UpdatePaymentIntetToSucceededOrFialed(string paymentIntentId, bool isSucceeded);
    }
}
