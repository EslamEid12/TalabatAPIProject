using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifiction;

namespace Talabat.Core.Repository
{
    public interface IGenaricRepository<T> where T : BaseEntity
    {
        Task<IReadOnlyList<T?>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> Spec);
        Task<T?> GetEntityWithSpecAsync(ISpecification<T> Spec);
        Task<int> GetCountAsync(ISpecification<T> spec);
        Task AddAsync(T entity);

        void Update(T entity);

        void Delete(T entity);
    }
}
