using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VardagshörnanApp.Models;

namespace VardagshörnanApp.Customer
{
    internal class AfterCheckOut
    {

        public static void AfterCheckoutMenu(Models.Customer customer)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Vad vill du göra nu?");
                Console.WriteLine("1. Tillbaka till shoppen");
                Console.WriteLine("2. Visa mina beställningar");
                Console.WriteLine("3. Logga ut");

                char choice = Console.ReadKey().KeyChar;
                Console.WriteLine();

                if (choice == '1')
                {
                    CustomerPage.CustomerMenuAsync(); 
                    break;
                }
                else if (choice == '2')
                {
                    ShowOrderHistory(customer);
                }
                else if (choice == '3')
                {
                    RegisterCustomer.LogOut();
                    Console.WriteLine("Du är nu utloggad.");
                    Thread.Sleep(1000);
                    break;
                }
                else
                {
                    Console.WriteLine("Fel val.");
                    Thread.Sleep(1000);
                }
            }
        }
        public static void ShowOrderHistory(Models.Customer customer)
        {
            using (var db = new MyDbContext())
            {
                var orders = db.Orders // kunds ordrar
                    .Where(o => o.CustomerId == customer.Id)
                    .Include(o=> o.OrderItems) // för att nå orderitems inne i ordrar
                    .ThenInclude(o=>o.Product) // för att nå information om produkterna
                    .ToList();

                Console.Clear();

                if (orders.Count==0)
                {
                    Console.WriteLine("Du har inga tidigare beställningar.");
                }
                else
                {
                    Console.WriteLine("Dina beställningar:");
                    Console.WriteLine("-------------------------------------------------------------------------------------------");

                    foreach (var order in orders)
                    {
                        Console.WriteLine("Order :" + order.Id + " - " + order.OrderDate + " - " + order.TotalAmount + " kr");
                        foreach (var item in order.OrderItems)
                        {
                            Console.WriteLine(" - " + item.Product.Name + " x " + item.Quantity + " = " + item.Price + " * " + item.Quantity + " kr");
                        }

                        Console.WriteLine("--------------------------------------------------------------------------------------------");
                    }
                }
            }

            Console.WriteLine("Tryck valfri tangent för att gå tillbaka.");
            Console.ReadKey();
        }
    }
}
