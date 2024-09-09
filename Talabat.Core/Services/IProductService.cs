using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifiction.ProductSpec;

namespace Talabat.Core.Services
{
    public interface IProductService
    {
        Task<IReadOnlyList<Product>> GetProductAsync(ProductSpecPram specPram);
        Task<Product> GetProductAsync(int id);
        Task<int> GetCountAsync(ProductSpecPram specPram);
        Task<IReadOnlyList<ProductBrand>> GetProductBrand();
        Task<IReadOnlyList<ProductType>> GetProductType();
    }
}
