using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Interfaces
{
    public interface IBasketRepo
    {
        public Task<CustomerBasket?> GetBasket(string id);
        public Task<CustomerBasket?> UpdateBasket (CustomerBasket backet);
        public Task<bool> DeleteBacket(string id);

    }



}
