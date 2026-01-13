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

                Console.WriteLine("Ange Produkt Id för ");
                // WindowHelper.CatalogWindow();
                //Console.SetCursorPosition(44, 32);
                //string searchedProduct = Console.ReadLine();
                //// fixa sökfältet
            }
            return;
        }
    
    }
}
