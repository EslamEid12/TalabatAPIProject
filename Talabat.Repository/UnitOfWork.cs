using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Repository;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private Hashtable _repositories;
        private readonly StoreDBContext _dBContext;

        //public IGenaricRepository<Product> ProductRepo { get ; set; }
        //public IGenaricRepository<ProductBrand> ProductBrandRepo { get ; set; }
        //public IGenaricRepository<ProductType> ProductTypeRepo { get ; set ; }
        //public IGenaricRepository<Order> OrderRepo { get ; set; }
        //public IGenaricRepository<DeliveryMethod> DeliveryMethodRepo { get; set ; }
        //public IGenaricRepository<OrderItem> OrderItemRepo { get ; set ; }

        public UnitOfWork(StoreDBContext dBContext)
        {
            _dBContext = dBContext;
            //ProductRepo = new GenaricRepository<Product>(_dBContext);
            //ProductBrandRepo = new GenaricRepository<ProductBrand>(_dBContext);
            //ProductTypeRepo = new GenaricRepository<ProductType>(_dBContext);
            //OrderRepo = new GenaricRepository<Order>(_dBContext);
            //DeliveryMethodRepo = new GenaricRepository<DeliveryMethod>(_dBContext);
            //OrderItemRepo = new GenaricRepository<OrderItem>(_dBContext);
        }

        public IGenaricRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            if (_repositories == null)
                _repositories = new Hashtable();

            var type = typeof(TEntity).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repository = new GenaricRepository<TEntity>(_dBContext);
                _repositories.Add(type, repository);
            }

            return _repositories[type] as IGenaricRepository<TEntity>;
        }
        public async Task<int> Complete()
        {
            return await _dBContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dBContext.Dispose();
        }


    }
}
