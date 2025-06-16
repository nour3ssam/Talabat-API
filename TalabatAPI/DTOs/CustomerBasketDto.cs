﻿using System.ComponentModel.DataAnnotations;
using Talabat.Core.Entities;

namespace TalabatAPI.DTOs
{
    public class CustomerBasketDto
    {
        [Required]
        public string Id { get; set; }

        public List<BasketItemDto> Item { get; set; }
        public string? PaymentIntentId { get; set; }
        public string? ClientSecret { get; set; }
        public int? DeliveryMethodId { get; set; }

    }
}
