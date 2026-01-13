using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VardagshörnanApp.Models
{
    internal class Product
    {
        public enum ProductStatus
        {
            Tillgänglig,
            SlutILager,
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; } = null!; // en till många
        public bool IsFeatured { get; set; } // utvalda på startsidan
        public int Stock { get; set; }
        public string Supplier { get; set; }
        public ProductStatus Status { get; set; } = ProductStatus.Tillgänglig; // tillgänglig per default (Enum)
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();// en till många, en product kan vara många orderItems 
        }
}
