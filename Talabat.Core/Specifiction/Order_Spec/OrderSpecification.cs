using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Core.Specifiction.Order_Spec
{
    public class OrderSpecification:BaseSpecification<Order>
    {
        public OrderSpecification(string buyeremail):base(O=>O.BuyerEmail==buyeremail) {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);
            AddOrderByDescending(O => O.OrderDate);
        
        }


        public OrderSpecification(int OrderId,string buyeremail) : base(O=>O.Id==OrderId &&O.BuyerEmail == buyeremail)
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);
        }
    }
}
