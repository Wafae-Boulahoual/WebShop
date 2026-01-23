using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VardagshörnanApp.Customer
{
    internal class Session
    {
        //En statisk klass för att komma ihåg vilken kund som är inloggad.
        public static Models.Customer LoggedInCustomer { get; set; }
        
    }
}
