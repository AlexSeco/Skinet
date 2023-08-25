using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;
using SkinetAPI.DTOs;
using SkinetAPI.Errors;

namespace SkinetAPI.Controllers;

public class ProductsController : BaseController
{
    private readonly IGenericRepository<Product> _productsRepo;
    private readonly IGenericRepository<ProductBrand> _brandsRepo;
    private readonly IGenericRepository<ProductType> _typesRepo;
    private readonly IMapper _mapper;

    public ProductsController
        (
        IGenericRepository<Product> productsRepo, 
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

        return Ok(_mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDTO>>(products));
    }


    [HttpGet("{id}")]
    [ProducesResponseType(200, Type = typeof(Product))]
    [ProducesResponseType(typeof(APIResponse), 404)]
    public async Task<ActionResult<ProductToReturnDTO>> GetProduct(int id)
    {
        ProductsWithTypesAndBrandsSpecification spec = new(id);

        Product product = await _productsRepo.GetEntityWithSpec(spec);

        if (product == null) return NotFound(new APIResponse(400));

        return _mapper.Map<Product, ProductToReturnDTO>(product);
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
