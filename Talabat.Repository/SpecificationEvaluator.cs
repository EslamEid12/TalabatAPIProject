using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifiction;

namespace Talabat.Repository
{
    public static class SpecificationEvaluator<T> where T : BaseEntity
    {
        public static IQueryable<T> GetQuery(IQueryable<T>Inputuery ,ISpecification<T> Spec)
        {
            // return (IEnumerable<T>)await  _context.Products.Include(p=>p.ProductBrand)
            // .Include(p=> p.ProductType).ToListAsync();

            var query = Inputuery; // _context.Products
            if (Spec.Criteria!=null)
                query = query.Where(Spec.Criteria); //_context.Products.where(p=>p.Id==Id)

            if (Spec.Orderby is not null) //p=>p.Name
                query = query.OrderBy(Spec.Orderby);

            else if (Spec.OrderbyDesending is not null) // p=>p.Price
                query = query.OrderByDescending(Spec.OrderbyDesending);
            if (Spec.IsPaginationEnable)
            {
                query = query.Skip(Spec.Skip).Take(Spec.Take);

            }

            query = Spec.Includes.Aggregate(query, (Currentquery, ExpressionQuery) => Currentquery.Include(ExpressionQuery));
            //_context.Products.where(p=>p.Id==Id).Include(p=>p.ProductBrand)
            //_context.Products.where(p=>p.Id==Id).Include(p=>p.ProductBrand).Include(p=> p.ProductType)


            return query;
        }

    }
}
