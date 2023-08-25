using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkinetAPI.DTOs;

namespace SkinetAPI.Controllers;

public class ProductsController : BaseController
{
    private readonly IGenericRepository<Products> _productsRepo;
    private readonly IGenericRepository<ProductBrand> _brandsRepo;
    private readonly IGenericRepository<ProductType> _typesRepo;
    private readonly IMapper _mapper;

    public ProductsController
        (
        IGenericRepository<Products> productsRepo, 
        IGenericRepository<ProductBrand> brandsRepo,
        IGenericRepository<ProductType> typesRepo,
        IMapper mapper
        )
    {
        _productsRepo = productsRepo;
        _brandsRepo = brandsRepo;
        _typesRepo = typesRepo;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ProductToReturnDTO>>> GetProducts()
    {
        ProductsWithTypesAndBrandsSpecification spec = new();

        var products = await _productsRepo.ListAsync(spec);

        return Ok(_mapper.Map<IReadOnlyList<Products>, IReadOnlyList<ProductToReturnDTO>>(products));
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<ProductToReturnDTO>> GetProduct(int id)
    {
        ProductsWithTypesAndBrandsSpecification spec = new(id);

        Products product = await _productsRepo.GetEntityWithSpec(spec);

        return _mapper.Map<Products, ProductToReturnDTO>(product);
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
    {
        return Ok(await _brandsRepo.ListAllAsync());
    }

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
    {
        return Ok(await _typesRepo.ListAllAsync());
    }


}
