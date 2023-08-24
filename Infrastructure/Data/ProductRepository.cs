using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ProductRepository : IProductRepository
{
    private readonly Context _db;

    public ProductRepository(Context db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync()
    {
        return await _db.ProductBrands.ToListAsync();
    }

    public async Task<Products> GetProductByIdAsync(int id)
    {
        return await _db.Products.Include(p => p.ProductBrand).Include(p => p.ProductType).FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IReadOnlyList<Products>> GetProductsAsync()
    {
        return await _db.Products.Include(p => p.ProductBrand).Include(p => p.ProductType).ToListAsync();
    }

    public async Task<IReadOnlyList<ProductType>> GetProductTypesAsync()
    {
        return await _db.ProductTypes.ToListAsync();
    }
}
