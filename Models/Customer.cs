using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VardagshörnanApp.Models
{
    internal class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string? EmailAdress { get; set; }
        public int? PhoneNumber { get; set; }
        public DateOnly BirthDate { get; set; }
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>(); // relation en till många som EF använder + bekräftade ordrar
        
    }
}
