using AutoMapper;
using Core.Entities;
using Core.Entities.Identity;
using Core.Entities.OrderAggregate;
using SkinetAPI.DTOs;

namespace SkinetAPI.Helpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Product, ProductToReturnDTO>()
            .ForMember(d => d.ProductBrand, o => o.MapFrom(s => s.ProductBrand.Name))
            .ForMember(d => d.ProductType, o => o.MapFrom(s => s.ProductType.Name))
            .ForMember(d => d.PictureUrl, o => o.MapFrom(new UrlResolver<Product, ProductToReturnDTO>("PictureUrl").Resolve));

        CreateMap<Core.Entities.Identity.Address, AddressDTO>().ReverseMap();
        
        CreateMap<CustomerBasketDTO, CustomerBasket>();

        CreateMap<BasketItemDTO, BasketItem>();

        CreateMap<AddressDTO, Core.Entities.OrderAggregate.Address>();

        CreateMap<Order, OrderToReturnDTO>()
            .ForMember(d => d.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod.ShortName))
            .ForMember(d => d.ShippingPrice, o => o.MapFrom(s => s.DeliveryMethod.Price));

        CreateMap<OrderItem, OrderItemDTO>()
            .ForMember(d => d.ProductId, o => o.MapFrom(s => s.ItemOrdered.ProductItemId))
            .ForMember(d => d.ProductName, o => o.MapFrom(s => s.ItemOrdered.ProductName))
            .ForMember(d => d.PictureUrl, o => o.MapFrom(s => s.ItemOrdered.PictureUrl))
            .ForMember(d => d.PictureUrl, o => o.MapFrom<OrderItemUrlResolver>());
    }

    public static class UrlResolverFactory
{
    public static UrlResolver<TSource, TDestination> Create<TSource, TDestination>(string propertyName)
    {
        return new UrlResolver<TSource, TDestination>(propertyName);
    }
}
    
    
        

    
}
