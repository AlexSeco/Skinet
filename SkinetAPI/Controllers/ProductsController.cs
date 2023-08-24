using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SkinetAPI.Controllers;

public class ProductsController : BaseController
{
    private readonly Context _db;

    public ProductsController(Context db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<List<Products>> GetProducts()
    {
        return await _db.Products.ToListAsync();
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<Products>> GetProduct(int id)
    {
        return await _db.Products.FindAsync(id);
    }
}
