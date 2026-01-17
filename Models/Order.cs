using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VardagshörnanApp.Models
{
    public enum OrderStatus
    {
        Väntande,
        Betald,
        // Orderstatus Avbruten och skickad har jag tagit bort eftersom den ingår inte i uppgiften
    }
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
        private const decimal VAT = 0.25m; // Moms 25% Value Added Taxe
        public int Id { get; set; }

        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; } = null!; // relation en till många en customer kan ha många orders

        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Väntande; // per default enum

        public ShippingMethod ShippingMethod { get; set; } // enum 
        public PaymentMethod PaymentMethod { get; set; } // enum
        public decimal ShippingCost { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>(); // relation en till många som EF använder
    }
}
