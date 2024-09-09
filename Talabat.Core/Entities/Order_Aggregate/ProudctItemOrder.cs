namespace Talabat.Core.Entities.Order_Aggregate
{
    public class ProudctItemOrder
    {
        public ProudctItemOrder()
        {

        }
        public ProudctItemOrder(int productId, string productName, string pictureUrl)
        {
            ProductId = productId;
            ProductName = productName;
            PictureUrl = pictureUrl;
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }
    }
}