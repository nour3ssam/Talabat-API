using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order;

namespace Talabat.Core.Specifications.OrderSpec
{
    public class OrderWithSpecification : BaseSpecifications<Order>
    {
        // To Get Order For User
        public OrderWithSpecification(string Email):base(o=>o.BuyerEmail==Email)
        {
            Includes.Add(o => o.DeliveryMethod);
            Includes.Add(o => o.Items);
            AddOrderBy(o=>o.OrderDate);
        }
        // To Get Order For User
        public OrderWithSpecification(string Email, int OrderId) : base(o => o.BuyerEmail == Email && o.Id == OrderId)
        {
            Includes.Add(o => o.DeliveryMethod);
            Includes.Add(o => o.Items);   
        }
    }
}
