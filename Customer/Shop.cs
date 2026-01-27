using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VardagshörnanApp.Admin;
using VardagshörnanApp.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace VardagshörnanApp.Customer
{
    internal class Shop
    {
        public static void ProductsForCustomer(int? categoryId = null) // kan vara null
        {
            while (true)
            {
                using (var db = new MyDbContext())
                {
                    Console.Clear();
                    List<Product> products; // hämtar alla produkter eller produkter i kategorier

                    if (categoryId == null) // om ingen parameter anges då visas alla produkter
                    {
                        products = db.Products
                            .Include(p => p.Category)
                            .ToList();
                    }
                    else // annars visar produkterna i kategorin med denna Id-n
                    {
                        products = db.Products 
                            .Include(p => p.Category)
                            .Where(p => p.CategoryId == categoryId)
                            .ToList();
                    }

                    Common.AllProductsTable(categoryId?? 0); // om ingen parameter => visas alla produkter
                                                             // om en parameter !=0 => visas produkterna i kategorin med denna Id-n

                    Console.WriteLine("Ange Produkt Id för mer detaljer (Q för att gå tillbaka):");
                    string input = Console.ReadLine();

                    if (input.ToLower() == "q")
                        break; // går tillbaka

                    if (!int.TryParse(input, out int IdToSee))
                    {
                        Console.ForegroundColor= ConsoleColor.Red;
                        Console.WriteLine("Ogiltigt ID! Försök igen.");
                        Console.ResetColor();
                        Console.ReadKey();
                        continue;
                    }
                    if (IdToSee == 0)
                    {
                        break;
                    }

                    // från products tar vi den produkten med den Id-n
                    var productToSee = products.SingleOrDefault(p => p.Id == IdToSee);

                    if (productToSee == null)
                    {
                        Console.ForegroundColor= ConsoleColor.Red;
                        Console.WriteLine("Produkten hittades inte i denna kategori!");
                        Console.ResetColor();
                        Console.ReadKey();
                        continue;
                    }

                    ProductDetailsForCustomer(productToSee);
                }
            }
        }
        public static void ProductDetailsForCustomer(Product product) // apposto
        {
            Console.Clear();
            string stockMessage;
            if (product.Stock > 5)
                stockMessage = "Tillgänglig";
            else if (product.Stock >= 1)
                stockMessage = "Få kvar i lager";
            else
                stockMessage = "Slut i lager";

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Produkt " + product.Name + " - Detaljer");
            Console.ResetColor();

            Console.WriteLine("============================================================================================================================================");
            Console.WriteLine(" ProduktId   : " + product.Id);
            Console.WriteLine(" Namn        : " + product.Name);
            Console.WriteLine(" Kategori    : " + product.Category.Name);
            Console.WriteLine(" Pris        : " + product.Price + " kr");
            Console.WriteLine(" Leverantör  : " + product.Supplier);
            Console.WriteLine(" LagerSaldo  : " + stockMessage);
            Console.WriteLine(" Beskrivning : " + product.Description);
            Console.WriteLine("============================================================================================================================================");
            Console.WriteLine();
            Console.WriteLine("Tryck Q för att gå tillbaka");
            Console.WriteLine("Tryck C för att lägga i varukorg");

            char choice = char.ToLower(Console.ReadKey().KeyChar);

            if (choice == 'c')
            {
                if (product.Stock == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Produkten är slut i lager och kan inte läggas till varukorgen.");
                    Console.ResetColor();
                    Console.ReadKey();
                    return;
                }
                else
                {
                    Cart.AddItemToCart(product);
                    //Console.ForegroundColor = ConsoleColor.Green;
                    //Console.WriteLine("Produkten har lagts till varukorgen!");
                    //Console.ResetColor();
                }
                Console.WriteLine("Tryck valfri tangent för fortsätta.");
                Console.ReadKey();
            }
            if (choice == 'q') 
                return;
        }
        public static void CustomerWindow() //apposto
        {

            List<string> topText11 = new List<string> { "A. Alla produkter", "V. Varukorg", "Q. Start sida" };
            var windowTop11 = new Window("Meny", 12, 25, topText11);
            windowTop11.Draw();

        }
        public static void ProductsIncategoriesWindow() // apposto
        {
            List<string> topText11 = new List<string> { "1. Hem & Dekoration", "2. Kök & Tillbehör ", "3. Böcker & Kontorsmaterial " };
            var windowTop11 = new Window("Kategorier: ", 57, 25, topText11);
            windowTop11.Draw();

        }
        public static async Task<List<Product>> TakeFeaturedProductsAsync()
        {
            using (var db = new MyDbContext())
            {
                var featuredProducts = await db.Products
                    .Include(p => p.Category)
                    .Where(p => p.IsFeatured)
                    .Take(3)
                    .ToListAsync();
                return featuredProducts;
            }
        }
        public static void FeaturedProductsWindows(List<Product>featuredProducts)
        { 
                //de tre utvalda produkter på start sidan
                List<string> window1Text = new List<string>
                {
                    featuredProducts[0].Name,
                    $"Pris: {featuredProducts[0].Price} kr",
                    "Tryck X för mer detaljer."
                };

                var window1 = new Window("Erbjudande 1", 8, 15, window1Text);
                window1.Draw();

                List<string> window2Text = new List<string>
                {
                    featuredProducts[1].Name,
                    $"Pris: {featuredProducts[1].Price} kr",
                    "Tryck Y för mer detaljer."
                };

                var window2 = new Window("Erbjudande 2", 58, 15, window2Text);
                window2.Draw();

                List<string> window3Text = new List<string>
                {
                    featuredProducts[2].Name,
                    $"Pris: {featuredProducts[2].Price} kr",
                    "Tryck Z för mer detaljer."
                };

                var window3 = new Window("Erbjudande 3", 108, 15, window3Text);
                window3.Draw();
;
            }
         // apposto
        public static void SearchProductCustomer() //apposto
        {
            var product = Common.SearchBarre();
            ProductDetailsForCustomer(product);

        }
      
    }
}

