using AutoMapper;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Entities.Order;
using TalabatAPI.DTOs;

namespace TalabatAPI.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() {

            CreateMap<Product, ProductToReturnDTO>()
                .ForMember(p => p.ProductType, o => o.MapFrom(s => s.ProductType.Name))
                .ForMember(p => p.ProductBrand, o => o.MapFrom(s => s.ProductBrand.Name))
                .ForMember(p => p.PictureURL, o => o.MapFrom<ProductUrlPictureResolve>());

            CreateMap<Address,AddressDto>().ReverseMap();
            CreateMap<CustomerBasket, CustomerBasketDto>().ReverseMap();
            CreateMap<BasketItem, BasketItemDto>().ReverseMap();
            CreateMap<AddressDto, OrderAddress>();


            CreateMap<Order,OrderToReturnDto>()
               .ForMember(p=>p.DeliveryMethod , o=>o.MapFrom(s=>s.DeliveryMethod.ShortName))
               .ForMember(p=>p.DeliveryMethodCost,o=>o.MapFrom(s=>s.DeliveryMethod.Cost));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(p => p.ProductId, o => o.MapFrom(s => s.Product.ProductId))
                .ForMember(p => p.ProductName, o => o.MapFrom(s => s.Product.ProductName))
                .ForMember(p => p.PictureUrl, o => o.MapFrom(s => s.Product.PictureUrl))
                .ForMember(p => p.PictureUrl, o => o.MapFrom<OrderItemPictureUrlResolve>());

        }
    }
}
