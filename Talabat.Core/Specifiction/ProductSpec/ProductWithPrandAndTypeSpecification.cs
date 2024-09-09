using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifiction.ProductSpec
{
    public class ProductWithPrandAndTypeSpecification : BaseSpecification<Product>
    {
        public ProductWithPrandAndTypeSpecification(ProductSpecPram specPram) : base(p => 
        (string.IsNullOrEmpty(specPram.Search) || p.Name.ToLower().Contains(specPram.Search)) &&
           (!specPram.brandId.HasValue || p.ProductBrandId == specPram.brandId.Value)
           && (!specPram.CategoryId.HasValue || p.ProductTypeId == specPram.CategoryId.Value)
        )
        {
            Includes.Add(p => p.ProductBrand);
            Includes.Add(p => p.ProductType);
            if (!string.IsNullOrEmpty(specPram.sort))
            {
                switch (specPram.sort)
                {
                    case "PriceAse":
                        AddOrderBy(p => p.Price);
                        break;
                    case "PriceDesc":
                        AddOrderByDescending(p => p.Price);
                        break;
                    default:
                        AddOrderBy(p => p.Name);
                        break;
                }
            }
            else
                AddOrderBy(p => p.Name);

            ApplyPagination((specPram.PageIndex - 1) * specPram.PageSize, specPram.PageSize);
        }

        public ProductWithPrandAndTypeSpecification(int id) : base(p => p.Id == id)
        {
            Includes.Add(p => p.ProductBrand);
            Includes.Add(p => p.ProductType);

        }
    }
}
