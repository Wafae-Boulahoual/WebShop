using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VardagshörnanApp.Models
{
    public enum PaymentMethod
    {
        Card,
        Invoice, 
        Swish
    }
    public enum ShippingMethod
    {
        Standard,
        Express
    }
    internal class Order
    {
        public const decimal VAT = 0.25m ; // Moms 25% 
        public int Id { get; set; }

        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; } = null!; // relation en till många 

        public DateTime OrderDate { get; set; }
        public ShippingMethod ShippingMethod { get; set; } // enum
        public PaymentMethod PaymentMethod { get; set; } // enum
        public decimal ShippingCost { get; set; }
        public decimal TotalAmount { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>(); // relation en till många 
    }
}
