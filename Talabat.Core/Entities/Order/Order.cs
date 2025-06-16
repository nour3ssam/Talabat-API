using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Order
{
    public class Order: BaseEntitiy
    {
        public Order()
        {
            
        }

        public Order(string buyerEmail, OrderAddress shippingAddress,DeliveryMethod deliveryMethod, ICollection<OrderItem> items, decimal subTotal,string paymentIntendId)
        {
            BuyerEmail = buyerEmail;
            ShippingAddress = shippingAddress;
            DeliveryMethod = deliveryMethod;
            Items = items;
            SubTotal = subTotal;
            PaymentIntendId= paymentIntendId;
        }

        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public OrderAddress ShippingAddress { get; set; } // Aggregation (Has a)
        public int DeliveryMethodId { get; set; }// FK
        public DeliveryMethod DeliveryMethod { get; set; } // Navigational property (1,m)
        public ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>(); // Navigational property (1,m)
        public decimal SubTotal { get; set; }

       /* [NotMapped]
        public decimal Total { get => SubTotal + DeliveryMethod.Cost; } // in runtime will collect */
        public Decimal GetTotal()
       => SubTotal + DeliveryMethod.Cost;
        public string PaymentIntendId { get; set; }

    }
}
