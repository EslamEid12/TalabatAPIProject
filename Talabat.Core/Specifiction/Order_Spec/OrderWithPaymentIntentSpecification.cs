using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Core.Specifiction.Order_Spec
{
    public class OrderWithPaymentIntentSpecification:BaseSpecification<Order>
    {
        public OrderWithPaymentIntentSpecification(string paymentIntntId): base (O=>O.PaymentIntntId==paymentIntntId)
        {

        }
    }
}
