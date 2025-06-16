using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order;
using Talabat.Core.Interfaces;
using Talabat.Core.ServiceInterfaces;

namespace Talabat.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration configuration;
        private readonly IBasketRepo basketRepo;
        private readonly IUnitOfWork unitOfWork;

        public PaymentService(IConfiguration _configuration,IBasketRepo _basketRepo,IUnitOfWork _unitOfWork)
        {
            configuration = _configuration;
            basketRepo = _basketRepo;
            unitOfWork = _unitOfWork;
        }
        public async Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string BasketId)
        {
            StripeConfiguration.ApiKey = configuration["StripeSettings:Secretkey"];

            var CustomerBasket = await basketRepo.GetBasket(BasketId);
            if (CustomerBasket is null) return null;

            var DeliveryMethodCost = 0M; // Decimal
            if (CustomerBasket.DeliveryMethodId.HasValue)
            {
                var DeliveryMethod = await unitOfWork.Repo<DeliveryMethod>().getByIdAsync(CustomerBasket.DeliveryMethodId.Value);
                DeliveryMethodCost = DeliveryMethod.Cost;
            }


            if (CustomerBasket.Item.Count > 0)
            {
                foreach (var item in CustomerBasket.Item)
                {
                    var product = await unitOfWork.Repo<Core.Entities.Product>().getByIdAsync(item.Id);
                    if (product.Price != item.Price)
                    {
                        item.Price = product.Price;
                    }
                }
            }


            var SubTotal = CustomerBasket.Item.Sum(Item=>Item.Price*Item.Quantity);



            var service = new PaymentIntentService();
            PaymentIntent PaymentIntent;
            if (string.IsNullOrEmpty(CustomerBasket.PaymentIntentId)) // Create for the first time
            {
                var options = new PaymentIntentCreateOptions()
                {
                    Amount = (long)SubTotal * 100 + (long)DeliveryMethodCost * 100,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string>() { "card" }
                };
                PaymentIntent = await service.CreateAsync(options);
                CustomerBasket.PaymentIntentId = PaymentIntent.Id;
                CustomerBasket.ClientSecret = PaymentIntent.ClientSecret;
            }
            else // Update 
            {
                var option = new PaymentIntentUpdateOptions() 
                {
                    Amount = (long)SubTotal * 100 + (long)DeliveryMethodCost * 100,
                };
                PaymentIntent=  await service.UpdateAsync(CustomerBasket.PaymentIntentId, option);
                CustomerBasket.PaymentIntentId = PaymentIntent.Id;
                CustomerBasket.ClientSecret = PaymentIntent.ClientSecret;

            }
         
            await basketRepo.UpdateBasket(CustomerBasket);
              return CustomerBasket;
        }
    }
}
