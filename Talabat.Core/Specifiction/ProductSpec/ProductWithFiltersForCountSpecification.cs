using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifiction.ProductSpec
{
    public class ProductWithFiltersForCountSpecification : BaseSpecification<Product>
    {
        public ProductWithFiltersForCountSpecification(ProductSpecPram productParams)
            : base(P =>
                    (string.IsNullOrEmpty(productParams.Search) || P.Name.ToLower().Contains(productParams.Search)) &&
                    (!productParams.brandId.HasValue || P.ProductBrandId == productParams.brandId.Value) &&
                    (!productParams.CategoryId.HasValue || P.ProductTypeId == productParams.CategoryId.Value)
            )
        {

        }
    }
}
