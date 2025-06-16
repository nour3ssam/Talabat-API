using System.ComponentModel.DataAnnotations;
using Talabat.Core.Entities.Order;

namespace TalabatAPI.DTOs
{
    public class OrderDto
    {
        [Required]
        public string CustomerBasketId { get; set; }
        [Required]
        public int DeliveryMethodId { get; set; }
        [Required] 
        public AddressDto ShippingAddress { get; set; }
    }
}
