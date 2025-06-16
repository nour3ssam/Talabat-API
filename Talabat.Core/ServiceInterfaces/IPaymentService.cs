using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.ServiceInterfaces
{
    public interface IPaymentService
    {
        // Create Or Update PaymentIntendId and  ClientSecret
        Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string BasketId);
    }
}
