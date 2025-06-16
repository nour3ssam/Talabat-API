using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Entities.Order;

namespace Talabat.Core.ServiceInterfaces
{
    public interface IOrderService
    {
        Task<Order?> CreateOrderAsync(string BuyerEmail, string BasketId, int DeliveryMethod, OrderAddress ShippingAddress);
        Task<IReadOnlyList<Order>> GetAllOrdersOfUserAsync(string BuyerEmail);
        Task<Order?> GetOrderByIdOfUserAsync(string BuyerEmail, int OrderId);
    }
}
