using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Order_Aggregate
{
    public class Order : BaseEntity
    {
        // There is must be Empty Parameterless Constructor here
        public Order()
        {

        }
        public Order(string buyerEmail, List<OrderItem> items, Address shipToAddress, DeliveryMethod deliveryMethod, decimal subTotal, string? paymentIntntId)
        {
            BuyerEmail = buyerEmail;
            Items = items;
            ShipToAddress = shipToAddress;
            DeliveryMethod = deliveryMethod;
            SubTotal = subTotal;
            PaymentIntntId = paymentIntntId;
        }

        public string BuyerEmail { get; set; }
        public ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>(); // Navigational Property [Many]
        public DateTimeOffset OrderDate { get; set; }
        public Address ShipToAddress { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; } // Navigational Property [ONE]
        public OrderStatus Status { get; set; }
        public decimal SubTotal { get; set; }

        public decimal GetTotal()
            => SubTotal + DeliveryMethod.Cost;
        public string? PaymentIntntId { get; set; }  
    }
}

