namespace TalabatAPIProject.Dtos
{
    public class OrderDto
    {
        //public string BuyerEmail { get; set; }

        public string BasketId { get; set; }
        public int DeliveryMethodId { get; set; }

        public AddressDto ShippingAddress { get; set; }
    }
}
