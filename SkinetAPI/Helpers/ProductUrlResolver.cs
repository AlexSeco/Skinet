using AutoMapper;
using AutoMapper.Execution;
using Core.Entities;
using SkinetAPI.DTOs;

namespace SkinetAPI.Helpers;

public class ProductUrlResolver : IValueResolver<Products, ProductToReturnDTO, string>
{
    private readonly IConfiguration _config;

    public ProductUrlResolver(IConfiguration config)
    {
        _config = config;
    }

    public string Resolve(Products source, ProductToReturnDTO destination, string destMember, ResolutionContext context)
    {
        if (!string.IsNullOrEmpty(source.PictureUrl))
        {
            return _config["ApiUrl"] + source.PictureUrl;
        }

        return null;
    }
}
