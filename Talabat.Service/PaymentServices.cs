using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Repository;
using Talabat.Core.Services;
using Talabat.Core.Specifiction.Order_Spec;
using Product = Talabat.Core.Entities.Product;

namespace Talabat.Service
{ 
    public class PaymentServices : IPaymentServices
    {
    private readonly IConfiguration _configuration;
    private readonly IBasketRepository _basketRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PaymentServices(IConfiguration configuration,IBasketRepository basketRepository,IUnitOfWork unitOfWork)
      {
             _configuration = configuration;
             _basketRepository = basketRepository;
             _unitOfWork = unitOfWork;
    }

        public async Task<CustomerBasket?> CreateOrUpadtePaymentIntent(string basketId)
        {
        StripeConfiguration.ApiKey =_configuration["StripeSettings:SecretKey"];
        var basket = await _basketRepository.GetBasketAsync(basketId);

        if (basket is null) return null;
            var ShippingPrice = 0m;
            if (basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(basket.DeliveryMethodId.Value);
                basket.ShippingPrice = deliveryMethod.Cost;
                ShippingPrice = deliveryMethod.Cost;
            }
        if (basket.Items.Count > 0)
        {
            foreach (var item in basket.Items)
            {
                var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                if ( item.Price!= product.Price) 
                    item.Price=product.Price;
            }
        }
            PaymentIntentService paymentIntentService = new PaymentIntentService();
            PaymentIntent paymentIntent;
            if (string.IsNullOrEmpty(basket.PaymentIntentId)) //create new Payment Intent
            {
                var createOptions = new PaymentIntentCreateOptions()
                {
                    Amount = (long)basket.Items.Sum(item => item.Price * item.Quantity * 100)
                    +(long)ShippingPrice*100,
                    Currency="usd",
                    PaymentMethodTypes=new List<string>() { "card"}
                };
                paymentIntent = await paymentIntentService.CreateAsync(createOptions);//Integrate with stripe
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else //Update new Payment Intent
            {
                var updateOptions = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)basket.Items.Sum(item => item.Price * item.Quantity * 100)
                      + (long)ShippingPrice * 100
                };
                await paymentIntentService.UpdateAsync(basket.PaymentIntentId, updateOptions);
            }

            await _basketRepository.UpdateBasketAsync(basket);
            return basket;
        }

        public async Task<Order> UpdatePaymentIntetToSucceededOrFialed(string paymentIntentId, bool isSucceeded)
        {
            var spec = new OrderWithPaymentIntentSpecification(paymentIntentId);
            var order=await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(spec);
            if (isSucceeded)
                order.Status = OrderStatus.PaymentReceieved;
            else
                order.Status = OrderStatus.PaymentFailed;
            _unitOfWork.Repository<Order>().Update(order);
            _unitOfWork.Complete();
            return order;

            
        }
    }
}
