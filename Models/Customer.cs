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
        // tog bort Gender propertin eftersom webbshoppen säljer heminredning
        // och kön är inte relevant i detta fall.
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string EmailAdress { get; set; }
        public string? PhoneNumber { get; set; }
        public DateOnly BirthDate { get; set; }
        public string? Password { get; set; }
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>(); // relation en till många som EF använder + bekräftade ordrar
        
    }
}
