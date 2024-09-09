using Talabat.Core.Entities.Order_Aggregate;

namespace TalabatAPIProject.Dtos
{
    public class OrderToReturnDto
    {
        public int Id { get; set; }

        public string BuyerEmail { get; set; }
        public ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>();
        public DateTimeOffset OrderDate { get; set; } 
        public Address ShipToAddress { get; set; }
        public string DeliveryMethod { get; set; }
        public decimal DeliveryMethodCost { get; set; }
        public string Status { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
    }
}
