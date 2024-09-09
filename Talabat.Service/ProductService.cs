using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repository;
using Talabat.Core.Services;
using Talabat.Core.Specifiction.ProductSpec;

namespace Talabat.Service
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IReadOnlyList<Product>> GetProductAsync(ProductSpecPram specPram)
        {
            var spec = new ProductWithPrandAndTypeSpecification(specPram);
            var product = await _unitOfWork.Repository<Product>().GetAllWithSpecAsync(spec);

            return product;
        }
        public async Task<Product> GetProductAsync(int ProductId)
        {
            var spec = new ProductWithPrandAndTypeSpecification(ProductId);
            var product = await _unitOfWork.Repository<Product>().GetEntityWithSpecAsync(spec);
            return product;
        }
        public async Task<int> GetCountAsync(ProductSpecPram specPram)
        {
            var CountSpec = new ProductWithFiltersForCountSpecification(specPram);
            var count = await _unitOfWork.Repository<Product>().GetCountAsync(CountSpec);
            return count;
        }

        public async Task<IReadOnlyList<ProductBrand>> GetProductBrand() 
            => await _unitOfWork.Repository<ProductBrand>().GetAllAsync();

        public async Task<IReadOnlyList<ProductType>> GetProductType()
        => await _unitOfWork.Repository<ProductType>().GetAllAsync();
    }
}
