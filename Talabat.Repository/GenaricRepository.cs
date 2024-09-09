using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repository;
using Talabat.Core.Specifiction;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class GenaricRepository<T> : IGenaricRepository<T> where T : BaseEntity
    {
        private readonly StoreDBContext _context;
        public GenaricRepository(StoreDBContext context)
        {
            _context = context;
        }  
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            //if (typeof(T)==typeof(Product))
            //    return (IEnumerable<T>)await  _context.Products.Include(p=>p.ProductBrand).Include(p=> p.ProductType).ToListAsync();
           
            //else
            //    return await _context.Set<T>().ToListAsync();
            return await _context.Set<T>().ToListAsync();
          
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
            
        }
        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> Spec)
        {
            return await ApplySpecification(Spec).ToListAsync();
        }

      

        public async Task<T> GetEntityWithSpecAsync(ISpecification<T> Spec)
        {
            return await ApplySpecification(Spec).FirstOrDefaultAsync();

        }

        public async Task<int> GetCountAsync(ISpecification<T> spec)
           => await ApplySpecification(spec).CountAsync();

        private  IQueryable<T>ApplySpecification(ISpecification<T> Spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_context.Set<T>(), Spec);
        }

        public async Task AddAsync(T entity)
       => await _context.Set<T>().AddAsync(entity);

        public void Update(T entity)
        => _context.Set<T>().Update(entity);

        public void Delete(T entity)
        => _context.Set<T>().Remove(entity);
    }


}
