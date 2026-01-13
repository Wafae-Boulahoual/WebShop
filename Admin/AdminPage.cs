using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VardagshörnanApp.Admin
{
    internal class AdminPage
    {
        public static void AdminMenu()
        {
            while (true)
            {
                Console.Clear();
                Helpers.WelcomeTextWindow();
                List<string> topText3 = new List<string> { "1. Hantera produkter", "2. Hantera kategorier", "3. Hantera kunder", "4. Se statistik(Queries)", "0. Tillbaka" };
                var windowTop3 = new Window("Vad vill du hantera?", 52, 6, topText3);
                windowTop3.Draw();

                if (int.TryParse(Console.ReadLine(), out int choice))
                {

                    switch (choice)
                    {
                        case 1:
                            ChangeProduct.ChangeProductsMenu();
                            break; // apposto
                        case 2:
                            ChangeCategory.ChangeCategoriesMenu();
                            break; // apposto
                        case 3:
                            //ChangeCustomer.ChangeCustomersMenu();
                            break;
                        case 4:
                            //ChangeCustomer.OrdersHistorik();
                            // ska använda dapper här
                            break;
                        case 0:
                            Console.Clear();
                            Helpers.WelcomeTextWindow();
                            Helpers.CustomerOrAdmin();
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Fel val! Försök igen");
                    return;
                }
            }

        }
    }

}


