﻿using Talabat.Core.Entities.Order;

namespace TalabatAPI.DTOs
{
    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }
        public int Quentity { get; set; }
    }
}
