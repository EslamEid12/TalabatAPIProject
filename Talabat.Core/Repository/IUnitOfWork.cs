using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Core.Repository
{
    public interface IUnitOfWork:IDisposable
    {
        //public IGenaricRepository<Product> ProductRepo { get; set; }
        //public IGenaricRepository<ProductBrand> ProductBrandRepo { get; set; }
        //public IGenaricRepository<ProductType> ProductTypeRepo { get; set; }
        //public IGenaricRepository<Order> OrderRepo { get; set; }
        //public IGenaricRepository<DeliveryMethod> DeliveryMethodRepo { get; set; }
        //public IGenaricRepository<OrderItem> OrderItemRepo { get; set; }

        IGenaricRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;
        Task<int> Complete();
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
