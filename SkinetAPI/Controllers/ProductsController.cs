using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SkinetAPI.Controllers;

public class ProductsController : BaseController
{
    private readonly IProductRepository _repo;

    public ProductsController(IProductRepository repo)
    {
        _repo = repo;
    }

    [HttpGet]
    public async Task<ActionResult<List<Products>>> GetProducts()
    {
        return Ok(await _repo.GetProductsAsync()); //This works because of the Ok()
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<Products>> GetProduct(int id)
    {
        return await _repo.GetProductByIdAsync(id);
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
    {
        return Ok(await _repo.GetProductBrandsAsync());
    }

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductTypes()
    {
        return Ok(await _repo.GetProductTypesAsync());
    }


}
