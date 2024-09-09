using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifiction
{
    public interface ISpecification<T> where T:BaseEntity
    {
        // return (IEnumerable<T>)await  _context.Products.Include(p=>p.ProductBrand).Include(p=> p.ProductType).ToListAsync();

        public Expression<Func<T, bool>> Criteria { get; set; }
        public List<Expression<Func<T, object>>>Includes { get;set; }
        public Expression<Func<T,object>>Orderby { get; set; }
        public Expression<Func<T, object>> OrderbyDesending { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }    
        
        public bool IsPaginationEnable { get; set; }

    }
}
