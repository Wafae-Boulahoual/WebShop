using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VardagshörnanApp.Models;

namespace VardagshörnanApp.Admin
{
    internal class ChangeCustomer
    {
        public static void ChangeCustomersMenu()
        {
        //    Console.Clear();
        //    while (true)
        //    {
        //        Console.Clear();
        //        Common.WelcomeTextWindow();
        //        //helpers.AllCustomers();
        //        List<string> topText8 = new List<string> { "1. Se Kundlistan", "2. Ändra kunduppgifter", "3. Se Beställninghistorik", "0. Tillbaka" };
        //        var windowTop8 = new Window("Välj en alternativ:", 52, 15, topText8);
        //        windowTop8.Draw();

        //        if (int.TryParse(Console.ReadLine(), out int choice))
        //        {
        //            switch (choice)
        //            {
        //                case 1:
        //                    Common.WelcomeTextWindow();
        //                    AllCustomersForAdmin();
        //                    break;
        //                case 2:
        //                    ChangeCustomersDetails();
        //                    break;
        //                case 3:
        //                    OrdersHistorik();
        //                    break;
        //                case 0:
        //                    return;
        //            }
        //        }
        //        else
        //        {
        //            Console.WriteLine("Försök igen!");
        //            return;
        //        }
        //        Console.ReadKey();
        //    }
        }
        public static void AllProductsForAdmin()
        {
            Console.Clear();
            Console.WriteLine("================================================================================");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("                            LINA SHOP - KATALOG                              ");
            Console.ResetColor();
            Console.WriteLine("================================================================================");
            Console.WriteLine($"|{"ProduktId",-10} | {"Produkt namn ",-20} | {"Pris",-10} | {"Lager",-4} | {"Leverantör",-10} | {"Utvald",-8}|");
            Console.WriteLine("--------------------------------------------------------------------------------");
            // skriva ut varje produkt
            using (var db = new MyDbContext())
            {
                foreach (var p in db.Products)
                {

                    Console.WriteLine($"|{p.Id,-10} | {p.Name,-20} | {p.Price,-10} | {p.Stock,-4} | {p.Supplier,-10} | {p.IsFeatured,-8} |");
                }
                Console.WriteLine("--------------------------------------------------------------------------------");

            }
            return;
        }
        public static void AllCustomersForAdmin()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=========================================================================================================================================");
                Console.WriteLine($"|{"Id",-3} |{"Namn",-25} |{"Stad",-15} |{"Email",-25} |{"Telefonnummer",-15} |{"Antal ordrar", -4}");
                Console.WriteLine("==========================================================================================================================================");

                using (var db = new MyDbContext())
                {
                    var customers = db.Customers
                                    .Include(c => c.Orders)
                                    .ToList();
                    foreach (var c in db.Customers)
                    {
                        Console.WriteLine($"|{c.Id,-3} |{c.FirstName} {c.LastName,-19} |{c.City, -15}|{c.EmailAdress,-25} |{c.PhoneNumber,-15} |{c.Orders.Count,-4}");
                        Console.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------");

                    }
                }
                Console.WriteLine("Tryck en valfri tangent för att fortsätta...");
                Console.ReadKey();
                return;
            }
        }
        public static void ChangeCustomersDetails()
        {
            while (true)
            {
                Console.Clear();
                Common.WelcomeTextWindow();
                AllCustomersForAdmin();
                Console.WriteLine("Vilken kund vill du ändra detaljer på? Ange Kund Id : ");
                if (!int.TryParse(Console.ReadLine(), out int ChosenId))
                {
                    Console.WriteLine("Fel Id! Försök igen.");
                    Thread.Sleep(1000);
                    continue; // börja om while loopen
                }
                Console.WriteLine("Vad vill du ändra? : ");
                Console.WriteLine("\n 1. Förnamn \n2.Efternamn \n3. Adress \n4. Stad \n5. Land \n6. Mejl adress \n7. Födelsdatum");
               
                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine("Fel val! Försök igen.");
                    continue; // börja om while loopen
                }
                using (var db = new MyDbContext())
                {
                    var CustomerToUpdate = (from c in db.Customers
                                            where c.Id == ChosenId
                                            select c).SingleOrDefault();
                    if (CustomerToUpdate == null)
                    {
                        Console.WriteLine("Kunden hittades inte!");
                        continue; // börja om while loopen
                    }
                    switch (choice)
                    {
                        case 1:
                            Console.WriteLine("Ange den nya Förenamn:");
                            CustomerToUpdate.FirstName = Console.ReadLine();
                            break;
                        case 2:
                            Console.WriteLine("Ange den nya Efternamn :");
                            CustomerToUpdate.LastName = Console.ReadLine();
                            break;
                        case 3:
                            Console.WriteLine("Ange den nya adressen: ");
                            CustomerToUpdate.Address = Console.ReadLine();
                            break;
                        case 4:
                            Console.WriteLine("Ange den nya staden: ");
                            CustomerToUpdate.City = Console.ReadLine();
                            break;
                        case 5:
                            Console.WriteLine("Ange Det nya landet :");
                            CustomerToUpdate.Country = Console.ReadLine();
                            break;
                        case 6:
                            Console.WriteLine("Ange den nya mejl adressen:");
                            CustomerToUpdate.EmailAdress = Console.ReadLine();
                            break;
                        case 7:
                            Console.WriteLine("Ange det nya födelsedatumet (yyyy-mm-dd): ");
                            DateOnly newBirthDate;
                            while (!DateOnly.TryParse(Console.ReadLine(), out newBirthDate))
                            {
                                Console.WriteLine("Ogiltigt datum. Försök igen (yyyy-mm-dd): ");
                            }
                            CustomerToUpdate.BirthDate = newBirthDate;
                            break;
                        default:
                            Console.WriteLine("Fel val! Försök igen");
                            break;
                    }
                    db.SaveChanges();
                    Console.WriteLine("Uppdateringen lyckades.");
                }
                Console.WriteLine("Tryck på valfri tangent för att fortsätta...");
                Console.ReadKey();
                break;
            }
        }
        public static void OrdersHistorik()
        {
            Console.Clear();
            AllCustomersForAdmin();
            Console.WriteLine("Ange Kund Id:");

            while (true)
            {
                if (!int.TryParse(Console.ReadLine(), out int customerId))
                {
                    Console.WriteLine("Fel val! Försök igen.");
                    continue;
                }
                using (var db = new MyDbContext())
                {
                    var orders = db.Orders
                        .Where(o => o.CustomerId == customerId)
                        .ToList();

                    if (orders.Count == 0)
                    {
                        Console.WriteLine("Inga ordrar hittades.");
                    }
                    else
                    {
                        foreach (var o in orders)
                        {
                            Console.WriteLine("-----------------------------------------------------------------------------------------------------");
                            Console.WriteLine("Order Id           : "+ o.Id);
                            Console.WriteLine("Datum              : "+ o.OrderDate);
                            Console.WriteLine("Betalning          : "+ o.PaymentMethod);
                            Console.WriteLine("Frakt              : "+ o.ShippingMethod);
                            Console.WriteLine("Totalt             : "+ o.TotalAmount);
                        }
                        Console.WriteLine("-----------------------------------------------------------------------------------------------------");
                        Console.WriteLine("Kunden har handlat: "+orders.Count+" gånger hos oss.");
                    }
                }
                Console.ReadKey();
                break;
            }

        }
        public static void ChangeCustomerWindow()
        {
            List<string> topText8 = new List<string> { "","X. Se Kundlistan", "Y. Ändra kunduppgifter", "Z. Se Beställninghistorik","" };
            Console.ForegroundColor = ConsoleColor.Green;
            var windowTop8 = new Window("Hantera en kund:", 80, 15, topText8);
            Console.ResetColor();
            windowTop8.Draw();
        }
    }
}
