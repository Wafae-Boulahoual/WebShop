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
        public static void ProductsForCustomer(int? categoryId = null)
        {
            while (true)
            {
                using (var db = new MyDbContext())
                {
                    Console.Clear();

                    List<Product> products; // tar alla produkter eller i kategorier

                    if (categoryId == null) // om ingen imparameter visar alla produkter
                    {
                        products = db.Products
                            .Include(p => p.Category)
                            .ToList();
                    }
                    else // annars visar prodykterna i kategorin med denna Id-n
                    {
                        products = db.Products 
                            .Include(p => p.Category)
                            .Where(p => p.CategoryId == categoryId)
                            .ToList();
                    }

                    Common.AllProductsTable(categoryId?? 0); // om ingen parameter visa alla
                                                             // om en parameter !=0 visa kategorins produkter

                    Console.WriteLine("Ange Produkt Id för mer detaljer (Q för att gå tillbaka):");

                    string input = Console.ReadLine();

                    if (input.ToLower() == "q")
                        break; // går tillbaka

                    if (!int.TryParse(input, out int IdToSee))
                    {
                        Console.WriteLine("Ogiltigt ID! Försök igen.");
                        Console.ReadKey();
                        continue;
                    }

                    if (IdToSee == 0)
                    {
                       // CustomerPage.CustomerMenu();
                        break;
                    }

                    // från products tar vi den produkten som vi vill
                    var productToSee = products.SingleOrDefault(p => p.Id == IdToSee);

                    if (productToSee == null)
                    {
                        Console.WriteLine("Produkten hittades inte i denna kategori!");
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
            Console.WriteLine($"Produkt {product.Name} - Detaljer");
            Console.ResetColor();

            Console.WriteLine("============================================================================================================================================");
            Console.WriteLine($" ProduktId   : {product.Id}");
            Console.WriteLine($" Namn        : {product.Name}");
            Console.WriteLine($" Kategori    : {product.Category.Name}");
            Console.WriteLine($" Pris        : {product.Price}");
            Console.WriteLine($" Leverantör  : {product.Supplier}");
            Console.WriteLine($" LagerSaldo  : {stockMessage}");
            Console.WriteLine($" Beskrivning : {product.Description}");
            Console.WriteLine("============================================================================================================================================");

            Console.WriteLine("Tryck Q för att gå tillbaka");
            Console.WriteLine("Tryck C för att lägga i varukorg");

            char choice = char.ToLower(Console.ReadKey().KeyChar);

            if (choice == 'c')
            {
               
                Cart.AddItemToCart(product);
                Thread.Sleep(1000);
            }
        }
        public static void CustomerWindow()
        {

            List<string> topText11 = new List<string> { "A. Alla produkter", "V. Varukorg", "Q. Start sida" };
            var windowTop11 = new Window("Meny", 12, 25, topText11);
            windowTop11.Draw();

        }
        public static void ProductsIncategoriesWindow()
        {
            List<string> topText11 = new List<string> { "1. Hem & Dekoration", "2. Kök & Tillbehör ", "3. Böcker & Kontorsmaterial " };
            var windowTop11 = new Window("Kategorier: ", 57, 25, topText11);
            windowTop11.Draw();

        }
        public static List<Product> FeaturedProductsWindows()
        {
            using (var db = new MyDbContext())
            {
                var featuredProducts = db.Products
                    .Include(p=>p.Category)
                    .Where(p => p.IsFeatured)
                    .Take(3)
                    .ToList();
                

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

                return featuredProducts;
            }
        } // apposto
        public static void SearchProductCustomer()
        {
            var product = Common.SearchBarre();
            ProductDetailsForCustomer(product);

        }
      
    }
}

