using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VardagshörnanApp
{
    internal class Session
    {
        //En statisk klass för att komma ihåg vilken kund eller admin som är inloggad.
        public static Models.Customer LoggedInCustomer { get; set; }
        public static Models.Administrator LoggedInAdministrator { get; set; }
        
    }
}
