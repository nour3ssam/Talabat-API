using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order;

namespace Talabat.Core.Specifications.OrderSpec
{
    public class OrderWithPaymentIntentIdSpecification:BaseSpecifications<Order>
    { 
        public OrderWithPaymentIntentIdSpecification(string PaymentIntentId):base(o=>o.PaymentIntendId == PaymentIntentId) 
        {
            
        }

    }
}
