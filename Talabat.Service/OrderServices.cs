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

namespace Talabat.Service
{
    public class OrderServices : IOrderServices
    {
        private readonly IBasketRepository _basketRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentServices _paymentServices;

        //private readonly IGenaricRepository<Product> _productsRepo;
        //private readonly IGenaricRepository<DeliveryMethod> _deliveryMethodsRepo;
        //private readonly IGenaricRepository<Order> _ordersRepo;

        public OrderServices(
            IBasketRepository basketRepo,
            //IGenaricRepository<Product> productsRepo,
            //IGenaricRepository<DeliveryMethod> deliveryMethodsRepo,
            //IGenaricRepository<Order> ordersRepo
            IUnitOfWork unitOfWork,
            IPaymentServices paymentServices
            )
        {
            _basketRepo = basketRepo;
            _unitOfWork = unitOfWork;
            _paymentServices = paymentServices;
            //_productsRepo = productsRepo;
            //_deliveryMethodsRepo = deliveryMethodsRepo;
            //_ordersRepo = ordersRepo;
        }

        public async Task<Order> CreateOrderAsync(string buyerEmail, string basketId, int deliverMethodId, Address shippingAddress)
        {
            // 1. Get Basket From Baskets Repo
            var basket = await _basketRepo.GetBasketAsync(basketId);

            // 2. Get Selected Items at Basket From Products Repo
            var orderItems = new List<OrderItem>();

            if (basket?.Items?.Count > 0) {
               

                foreach (var item in basket.Items)
                {

                    var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                    var productItemOrdered = new ProudctItemOrder(item.Id, product.Name, product.PictureUrl);

                    var orderItem = new OrderItem(productItemOrdered, product.Price, item.Quantity);
                    orderItems.Add(orderItem);
                }
            }

            // 3. Calculate SubTotal
            var subTotal = orderItems.Sum(item => item.Price * item.Quantity);

            // 4. Get Delivery Method From DeliveryMethods Repo
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliverMethodId);

            var orderRepo = _unitOfWork.Repository<Order>();
            var orderSpec = new OrderWithPaymentIntentSpecification(basket.PaymentIntentId);
            var existOrder = await orderRepo.GetEntityWithSpecAsync(orderSpec);
            if (existOrder !=null)
            {
                orderRepo.Delete(existOrder);
                await _paymentServices.CreateOrUpadtePaymentIntent(basketId);
            }
            // 5. Create Order
            var order = new Order(buyerEmail, orderItems, shippingAddress, deliveryMethod, subTotal,basket.PaymentIntentId);
            //await _unitOfWork.Repository<Order>().AddAsync(order);
              await orderRepo.AddAsync(order);

            // 6. Save To Database [TODO]

            var result = await _unitOfWork.Complete();
            if (result <= 0) return null;

            return order;
        }
        public async Task<IReadOnlyList<Order>>GetOrdersForUser(string buyerEmail)
        {
            var OrderRepo= _unitOfWork.Repository<Order>();
            var spec = new OrderSpecification(buyerEmail);
            var Orders = await OrderRepo.GetAllWithSpecAsync(spec);
            return Orders;
        }
        public Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodAsync()
        {
            var deliveryMethodRepo=_unitOfWork.Repository<DeliveryMethod>();
            var deliveryMethod = deliveryMethodRepo.GetAllAsync();
            return deliveryMethod;
        }

        public async Task<Order?> GetOrderByIdForUser(int orderId, string buyerEmail)
        {
            var OrderRepo= _unitOfWork.Repository<Order>();
            var spec = new OrderSpecification(orderId, buyerEmail);
            var Order = await OrderRepo.GetEntityWithSpecAsync(spec);
            return Order;
        }

    }
}
