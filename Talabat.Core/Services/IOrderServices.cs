using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Core.Services
{
    public interface IOrderServices
    {
        Task<Order> CreateOrderAsync(string buyerEmail, string basketId, int deliverMethodId, Address shippingAddress);

        Task<IReadOnlyList<Order>> GetOrdersForUser(string buyerEmail);

        Task<Order?> GetOrderByIdForUser(int orderId, string buyerEmail);

        Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodAsync();
    }
}
