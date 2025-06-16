using AutoMapper;
using Talabat.Core.Entities.Order;
using TalabatAPI.DTOs;

namespace TalabatAPI.Helpers
{
    public class OrderItemPictureUrlResolve : IValueResolver<OrderItem, OrderItemDto, string>
    {
        private readonly IConfiguration configuration;

        public OrderItemPictureUrlResolve(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.Product.PictureUrl))
            {
                return $"{configuration["BaseUrl"]}{source.Product.PictureUrl}";        
            }
            return string.Empty;
        }

            
        
    }
}
