using AutoMapper;
using Talabat.Core.Entities.Order_Aggregate;
using TalabatAPIProject.Dtos;

namespace TalabatAPIProject.Helpers
{
    public class OrderItemPictureUrlResolver:IValueResolver<OrderItem,OrderItemDto,string>
    {
        private readonly IConfiguration _configuration;

        public OrderItemPictureUrlResolver(IConfiguration configuration )
        {
            _configuration = configuration;
        }

        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.ProductItemOrdered.PictureUrl))
                return $"{_configuration["ApiUrl"]}{source.ProductItemOrdered.PictureUrl}";
            return string.Empty;
        }
    }
}
