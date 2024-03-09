using AutoMapper;
using Core.Entities.OrderAggregate;
using SkinetAPI.DTOs;

namespace SkinetAPI.Helpers;

public class OrderItemUrlResolver : IValueResolver<OrderItem, OrderItemDTO, string>
{
    private readonly IConfiguration _config;

    public OrderItemUrlResolver(IConfiguration config)
    {
        _config = config;
    }

    public string Resolve(OrderItem source, OrderItemDTO destination, string destMember, ResolutionContext context)
    {
        if (!string.IsNullOrEmpty(source.ItemOrdered.PictureUrl))
        {
            return _config["ApiUrl"] + source.ItemOrdered.PictureUrl;
        }

        return null;
    }
}