using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Entities.Order_Aggregate;
using TalabatAPIProject.Dtos;
using TalabatAPIProject.Helpers;

namespace Talabat.Core.Helpers
{
    public class MappingProfile:Profile 
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductToReturnDto>()
                    .ForMember(d => d.ProductType, o => o.MapFrom(s => s.ProductType.Name))
                    .ForMember(d => d.ProductBrand, o => o.MapFrom(s => s.ProductBrand.Name))
                    .ForMember(d => d.PictureUrl, o => o.MapFrom<ProductPictureUrlResolver>());
            CreateMap<CustomerBasketDto, CustomerBasket>();
            CreateMap<BasketItemDto, BasketItem>();
            //Order_Aggregate
            CreateMap<AddressDto,Core.Entities.Order_Aggregate.Address>().PreserveReferences();
            //Identity
            CreateMap<Core.Entities.Identity.Address, AddressDto>();
            CreateMap<Order, OrderToReturnDto>()
                .ForMember(d => d.DeliveryMethod, O => O.MapFrom(s => s.DeliveryMethod.ShortName))
                .ForMember(d => d.DeliveryMethod, O => O.MapFrom(s => s.DeliveryMethod.ShortName));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d => d.ProductId, O => O.MapFrom(s => s.ProductItemOrdered.ProductId))
                .ForMember(d => d.ProductName, O => O.MapFrom(s => s.ProductItemOrdered.ProductName))
                .ForMember(d => d.PictureUrl, O => O.MapFrom(s => s.ProductItemOrdered.PictureUrl))
                .ForMember(d=>d.PictureUrl,O=>O.MapFrom<OrderItemPictureUrlResolver>());


        }
    }
}
