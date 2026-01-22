using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VardagshörnanApp.Admin
{
    internal class Statistics
    {
        public static void StatisticsWindow()
        {
            List<string> topText = new List<string> { "","W. Statistik", "Q. Start sida","" };
            Console.ForegroundColor = ConsoleColor.Green;
            var windowTop = new Window("Välj en alternativ", 112, 15, topText);
            Console.ResetColor();
            windowTop.Draw();
        }
        public static void Queries()
        {
           
        }
    }
}
