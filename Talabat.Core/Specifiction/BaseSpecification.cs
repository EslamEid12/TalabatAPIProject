using System.Linq.Expressions;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifiction
{

    public class BaseSpecification<T> : ISpecification<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>> Criteria { get; set; }
        public List<Expression<Func<T, object>>> Includes { get; set;}=new List<Expression<Func<T, object>>>();
        public Expression<Func<T, object>> Orderby { get ; set ; }
        public Expression<Func<T, object>> OrderbyDesending { get; set ; }
        public int Skip { get ; set; }
        public int Take { get ; set; }
        public bool IsPaginationEnable { get ; set; }

        public BaseSpecification()
        {
                
        }

        public BaseSpecification(Expression<Func<T, bool>> _criteria)
        {
                Criteria=_criteria;  
        }
        public void AddOrderBy(Expression<Func<T, object>> OrderbyExpression)
        {
            Orderby = OrderbyExpression;
        }
        public void AddOrderByDescending(Expression<Func<T, object>> OrderbyDesendingExpression)
        {
            OrderbyDesending = OrderbyDesendingExpression;
        }
        public void ApplyPagination(int skip,int take)
        {
            IsPaginationEnable = true;
            Skip = skip;
            Take=take;
        }
    }

}
