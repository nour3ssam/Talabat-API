using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order;
using Talabat.Core.Interfaces;
using Talabat.Core.ServiceInterfaces;
using Talabat.Core.Specifications.OrderSpec;

namespace Talabat.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepo basketRepo;   
        private readonly IUnitOfWork unitOfWork;
        private readonly IPaymentService paymentService;

        public OrderService(IBasketRepo _basketRepo,IUnitOfWork UnitOfWork,IPaymentService _paymentService)
        {
            basketRepo = _basketRepo;
            unitOfWork = UnitOfWork;
            paymentService = _paymentService;
        }
        public async Task<Order?> CreateOrderAsync(string BuyerEmail, string CustomerBasketId, int DeliveryMethodId, OrderAddress ShippingAddress)
        {
            // Get Basket From Basket Repo
            var CustomerBasket = await basketRepo.GetBasket(CustomerBasketId);

            // Get Selected Item at Basket From Product Repo
            var OrderItems = new List<OrderItem>();
            if (CustomerBasket?.Item != null)
            {
                foreach (var item in CustomerBasket.Item)
                {
                    var product = await unitOfWork.Repo<Product>().getByIdAsync(item.Id);
                    var ProductItemOrder = new ProductItemOrdered(product.Id, product.Name, product.PictureURL);
                    var OrderItem = new OrderItem(ProductItemOrder, product.Price, item.Quantity);
                    OrderItems.Add(OrderItem);
                }
            }
            // Calculate SubTotal
            var SubTotal = OrderItems.Sum(Item=>Item.Price*Item.Quentity);

            // Get DeliveryMethod from DeliveryMethodRepo (We Use IGenaricRepo)
            var DeliveryMethod = await unitOfWork.Repo<DeliveryMethod>().getByIdAsync(DeliveryMethodId);


            // Create Order 
            var spec = new OrderWithPaymentIntentIdSpecification(CustomerBasket.PaymentIntentId);
            var ExOrder = await unitOfWork.Repo<Order>().GetEntityWithSpecAsync(spec);
            if (ExOrder is not null)
            {
                unitOfWork.Repo<Order>().DeleteAsync(ExOrder);
               await paymentService.CreateOrUpdatePaymentIntent(CustomerBasket.Id);
            }
            var Order = new Order(BuyerEmail, ShippingAddress, DeliveryMethod, OrderItems, SubTotal, CustomerBasket.PaymentIntentId);


            // Add Order Locally
             await unitOfWork.Repo<Order>().AddAsync(Order);

            // Save Order To DB
            var result = await unitOfWork.CompleteAsync();
            if (result == 0) return null;
            return Order;

        }

        public async Task<IReadOnlyList<Order>> GetAllOrdersOfUserAsync(string BuyerEmail)
        {
            var Spec = new OrderWithSpecification(BuyerEmail);
            var orders = await unitOfWork.Repo<Order>().GetAllWithSpecAsync(Spec);
            return orders;
        }

        public async Task<Order?> GetOrderByIdOfUserAsync(string BuyerEmail, int OrderId)
        {
            var Spec = new OrderWithSpecification(BuyerEmail,OrderId);
            var orders = await unitOfWork.Repo<Order>().GetEntityWithSpecAsync(Spec);
            return orders;
        }
    }
} 
