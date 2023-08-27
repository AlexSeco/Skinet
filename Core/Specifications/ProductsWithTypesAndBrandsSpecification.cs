using Core.Entities;

namespace Core.Specifications;

public class ProductsWithTypesAndBrandsSpecification : BaseSpecification<Product>
{
    public ProductsWithTypesAndBrandsSpecification(ProductSpecParams productParams) : base(x => 
    (string.IsNullOrEmpty(productParams.Search) || x.Name.ToLower().Contains(productParams.Search)) &&
    (!productParams.BrandId.HasValue || x.ProductBrandId == productParams.BrandId) && 
    (!productParams.TypeId.HasValue || x.ProductTypeId == productParams.TypeId)
    )
    {
        AddInclude(x => x.ProductType);
        AddInclude(x => x.ProductBrand);
        AddOrderby(x => x.Name);

        ApplyPaging(productParams.PageSize * (productParams.PageIndex - 1), productParams.PageSize);


        if (!string.IsNullOrEmpty(productParams.Sort))
        {
            switch (productParams.Sort)
            {
                case "priceAsc":
                    AddOrderby(p => p.Price);
                    break;

                case "priceDesc":
                    AddOrderbyDescending(p => p.Price);
                    break;

                default:
                    AddOrderby(n => n.Name);
                    break;
            }
        }
    }

    public ProductsWithTypesAndBrandsSpecification(int id) : base(x => x.Id == id) // First instantiation everytime this constructor is used
    {
        AddInclude(x => x.ProductType);
        AddInclude(x => x.ProductBrand);
    }
}
