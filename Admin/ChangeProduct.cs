using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VardagshörnanApp.Customer;
using VardagshörnanApp.Models;

namespace VardagshörnanApp.Admin
{
    internal class ChangeProduct // apposto
    {
        public static void ChangeProductWindow() // apposto
        {
            List<string> topText3 = new List<string> { "","1. Se alla produkter","2. Lägga till en ny produkt", "3. Ändra en detalj ","4. Ta bort en produkt","5. Ändra Utvalda produkter" ,"" };
            Console.ForegroundColor = ConsoleColor.Green;
            var windowTop3 = new Window("Hantera en produkt:", 3, 15, topText3);
            Console.ResetColor();
            windowTop3.Draw();
        }
        public static void AllProductsForAdmin() // apposto
        {
            while (true)
            {
                using (var db = new MyDbContext())
                {
                    Common.AllProductsTable();
                    Console.WriteLine("Ange Produkt Id för mer detaljer (Q för att gå tillbaka):");
                    string input = Console.ReadLine();

                    if (input.ToLower() == "q")
                    {
                        break; ; //gå tillbaka
                    }
                    if (!int.TryParse(input, out int IdToSee))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Ogiltigt ID! Försök igen.");
                        Console.ResetColor();
                        Console.ReadKey();
                        continue; // börja om loopen
                    }

                    var products = db.Products
                               .Include(p => p.Category).ToList();

                    var productToSee = products.SingleOrDefault(p => p.Id == IdToSee);

                    if (productToSee == null)
                    {
                        Console.ForegroundColor= ConsoleColor.Red;
                        Console.WriteLine("Produkten hittades inte! Försök igen.");
                        Console.ResetColor();
                        Console.ReadKey();
                        continue;
                    }
                    ProductDetailsForAdmin(productToSee);
                }
            }
        }
        public static void ProductDetailsForAdmin(Product product) 
        {
            // om produkten är null finns inget att visa
            if (product == null) return;
            
            string featuredOrNot = product.IsFeatured ? "Utvald på start sida" : "Ej utvald på start sida";

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Produkt " + product.Name + " Beskrivning :");
            Console.ResetColor();

            Console.WriteLine("====================================================================================================================================");
            Console.WriteLine("ProduktId     : " + product.Id);
            Console.WriteLine("Namn          : " + product.Name);
            Console.WriteLine("Kategori Id   : " + product.CategoryId);
            Console.WriteLine("Kategori Namn : " + product.Category.Name);
            Console.WriteLine("Pris          : " + product.Price);
            Console.WriteLine("I lager       : " + product.Stock);
            Console.WriteLine("Leverantör    : " + product.Supplier);
            Console.WriteLine("Status        : " + featuredOrNot);
            Console.WriteLine("Beskrivning   : " + product.Description);
            Console.WriteLine("======================================================================================================================================");
            Console.WriteLine("Tryck en valfri tangent för att gå tillbaka");
            Console.ReadKey();

        }  // apposto
        public static void AddProduct()
        {
            Console.Clear();
            Console.WriteLine("Lägga till en ny Produkt");
            Console.WriteLine("------------------------");
            Console.WriteLine("Ange Produktens detaljer :");
            Console.WriteLine("Produkt namn:");
            string name = Console.ReadLine();
            Console.WriteLine("Detaljerad information :");
            string detail = Console.ReadLine();
            Console.WriteLine("Pris:");
            decimal price;
            while (!decimal.TryParse(Console.ReadLine(), out price))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ogiltig pris! försök igen");
                Console.ResetColor();
            }
            Console.WriteLine("Kategori ID: ");
            int categoryId;
            while(!int.TryParse(Console.ReadLine(), out categoryId))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ogiltig Id! försök igen");
                Console.ResetColor();
            }
            Console.WriteLine("Vill du att produkten ska på start sidan? ja eller nej:");
            bool featured;
            while (true)
            {
                string input = Console.ReadLine().ToLower();
                if (input == "ja")
                {
                    featured = true;
                    break;
                }
                else if (input == "nej")
                {
                    featured = false;
                    break;
                }
                else
                {
                    Console.WriteLine("Skriv 'ja' eller 'nej':");
                }
            }
            Console.WriteLine("Hur många på lager?");
            int stock;
            while(!int.TryParse(Console.ReadLine(), out stock))
            { 
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ogiltigt antal! försök igen");
                Console.ResetColor();
            }
            Console.WriteLine("Leverantörs namn:");
            string supplier = Console.ReadLine();
            using (var db = new MyDbContext())
            {
                var newProduct = new Product
                {
                    Name = name,
                    Description = detail,
                    Price = price,
                    CategoryId = categoryId,
                    IsFeatured = featured,
                    Stock = stock,
                    Supplier = supplier
                };

                try
                {
                    db.Products.Add(newProduct);
                    db.SaveChanges();
                    Console.WriteLine("Produkten har lagts till i webbshoppen.");
                }
                catch(Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Den nya produkten kunde inte läggas till.");
                    Console.ResetColor();
                }
            }
            Thread.Sleep(1000);
            Console.ReadKey();
        }  // apposto
        public static void ChangeDetails()
        {
            while (true)
            {
                Console.Clear();
                Common.WelcomeTextWindow();
                Common.AllProductsTable();
                Console.WriteLine("Vilken produkt vill du ändra detaljer på? Ange Produkt Id : ");
                if (!int.TryParse(Console.ReadLine(), out int ChosenId))
                {
                    Console.WriteLine("Fel Id! Försök igen.");
                    Thread.Sleep(1000);
                    continue; // börja om while loopen
                }
                Console.WriteLine("Vad vill du ändra?\n 1. Namn\n 2. Beskrivning \n3. Pris \n4. ProduktKategori\n5. Leverantör \n6. Lager");
                
                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine("Fel val! Försök igen.");
                    continue; // börja om while loopen
                }
                using (var db = new MyDbContext())
                {
                    var ProductToUpdate = (from p in db.Products
                                           where p.Id == ChosenId
                                           select p).SingleOrDefault();
                    if (ProductToUpdate == null)
                    {
                        Console.WriteLine("Produkten hittades inte!");
                        Thread.Sleep(1000);
                        continue; // continue börjar om while loopen
                    }
                    switch (choice)
                    {
                        case 1:
                            Console.WriteLine("Ange den nya namnet:");
                            ProductToUpdate.Name = Console.ReadLine();
                            break;
                        case 2:
                            Console.WriteLine("Ange den nya beskrivning :");
                            ProductToUpdate.Description = Console.ReadLine();
                            break;
                        case 3:
                            Console.WriteLine("Ange den nya priset: ");
                            decimal newPrice;
                            while (!decimal.TryParse(Console.ReadLine(), out newPrice))
                            {
                                Console.WriteLine("Ogiltigt pris. Försök igen:");
                            }
                            ProductToUpdate.Price = newPrice;
                            break;
                        case 4:
                            Console.WriteLine("Ange den nya ProduktKategori: ");
                            var categories = db.Categories.ToList();
                            //om vi lägger en ny kategori den visas här
                            for (int i = 0; i < categories.Count; i++)
                            {
                                Console.WriteLine(categories[i].Id+". "+ categories[i].Name);
                            }
                            int newId;
                            while (!int.TryParse(Console.ReadLine(), out newId))
                            {
                                Console.WriteLine("Ogiltigt Id. Försök igen: ");
                            }
                            ProductToUpdate.CategoryId = newId;
                            break;
                        case 5:
                            Console.WriteLine("Ange Den nya leverantören :");
                            ProductToUpdate.Supplier = Console.ReadLine();
                            break;
                        case 6:
                            Console.WriteLine("Ange det nya lagersaldot:");
                            int newStock;
                            while (!int.TryParse(Console.ReadLine(), out newStock))
                            {
                                Console.WriteLine("Ogiltigt Saldo. Försök igen : ");
                            }
                            ProductToUpdate.Stock = newStock;
                            break;
                        default:
                            Console.WriteLine("Fel val! Försök igen");
                            Thread.Sleep(1000);
                            continue;
                    }
                    try
                    {
                        db.SaveChanges();
                        Console.WriteLine("Uppdateringen lyckades.");
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine("Uppdatering lyckades inte! ");
                        Console.WriteLine("Fel: " + ex.Message);
                        Thread.Sleep(1500);
                        continue;
                    }
                }
                Console.WriteLine("Tryck på valfri tangent för att fortsätta...");
                Console.ReadKey();
                return;
            }

        } // apposto
        public static void DeleteAProduct()
        {

            Console.Clear();
            AllProductsForAdmin();
            Console.WriteLine("Ange vilken Id Produkt vill du ta bort: ");
            if (!int.TryParse(Console.ReadLine(), out int idToCancel))
            {
                Console.WriteLine("Fel Id, Försök igen!");
                Console.ReadKey();
                return;
            }
            using (var db = new MyDbContext())
            {
                var deleteProduct = (from t in db.Products
                                     where t.Id == idToCancel
                                     select t).SingleOrDefault();
                if (deleteProduct == null)
                {
                    Console.WriteLine("Produkten hittades inte");
                    Console.ReadKey();
                    return;
                }
                try
                {
                    db.Products.Remove(deleteProduct);
                    db.SaveChanges();
                    Console.WriteLine("Produkten togs bort");
                    Thread.Sleep(1000);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Produkten kunde inte tas bort...");
                    Console.WriteLine("Fel : " + ex.Message);
                }
            }
            Console.ReadKey();
        } // apposto
        
        public static void SearchProductAdmin() // apposto
        {
            var product = Common.SearchBarre(); 
            ProductDetailsForAdmin(product);
        }
        public static void ChangeFeaturedProducts() // apposto
        {
            Console.Clear();
            Console.WriteLine("Utvalda produkter på start sida :");
            using (var db = new MyDbContext())
            {
                var featuredProducts = db.Products
                    .Include(p => p.Category)
                    .Where(p => p.IsFeatured)
                    .Take(3) // tre utvalda produkter som står på start sidan
                    .ToList();
                foreach (var product in featuredProducts)
                {
                    Console.WriteLine("Produkt namn: " + product.Name);
                    Console.WriteLine("Produkt Id :" + product.Id);
                    Console.WriteLine("Pris :" + product.Price);
                    Console.WriteLine("Kategori : " + product.CategoryId + " " + product.Category.Name);
                    Console.WriteLine("-------------------------------------------------------------------------------");
                }

                Console.WriteLine("Vilken produkt vill du ta bort från startsidan? Ange Produkt ID:");
                if (!int.TryParse(Console.ReadLine(), out int oldId))
                {
                    Console.ForegroundColor= ConsoleColor.Red;
                    Console.WriteLine("Felaktigt ID.");
                    Console.ResetColor();
                    return;
                }
                Console.Clear();
                Common.AllProductsTable();
                Console.WriteLine("Vilken produkt vill du lägga till på startsidan? Ange Produkt ID:");
                if (!int.TryParse(Console.ReadLine(), out int newId))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Felaktigt ID.");
                    Console.ResetColor ();
                    return;
                }
                var oldProduct = db.Products.SingleOrDefault(p => p.Id == oldId);
                var newProduct = db.Products.SingleOrDefault(p => p.Id == newId);

                if (oldProduct == null || newProduct == null)
                {
                    Console.ForegroundColor=ConsoleColor.Red;
                    Console.WriteLine("En eller båda produkterna finns inte.");
                    Console.ResetColor();
                    return;
                }
                oldProduct.IsFeatured = false;
                newProduct.IsFeatured = true;
                try
                {
                    db.SaveChanges();
                    Console.WriteLine("Startsidan har uppdaterats korrekt.");
                    Shop.FeaturedProductsWindows();
                    Console.ReadKey();
                }
                catch(Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Uppdatering lyckades inte!");
                    Console.WriteLine(ex.Message);
                    Console.ResetColor();
                }
            }
            Console.WriteLine("Tryck valfri tangent för att fortsätta.");
            Console.ReadKey();
        }

    }
}

