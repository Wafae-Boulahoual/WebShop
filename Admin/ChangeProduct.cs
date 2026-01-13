using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VardagshörnanApp.Models;

namespace VardagshörnanApp.Admin
{
    internal class ChangeProduct
    {
        public static void ChangeProductsMenu()
        {
            Console.Clear();
            while (true)
            {
                Console.Clear();
                Helpers.WelcomeTextWindow();
                List<string> topText3 = new List<string> { "1. Se alla produkter", "2. Lägga till en ny produkt", "3. Ändra en detalj ", "4. Ta bort en product", "0. Tillbaka" };
                var windowTop3 = new Window("Hantera produkter:", 52, 6, topText3);
                windowTop3.Draw();

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    switch (choice)
                    {
                        case 1:
                            AllProductsForAdmin();
                            break;
                        case 2:
                            AddProduct();
                            AllProductsForAdmin();
                            break;
                        case 3:
                            ChangeDetails();
                            AllProductsForAdmin();
                            break;
                        case 4:
                            DeleteAProduct();
                            AllProductsForAdmin();
                            break;
                        case 0:
                            return;
                        default:
                            Console.WriteLine("Fel val!");
                            Console.ReadKey();
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Fel val! Försök igen");
                    return;
                }

            }
        } // apposto
        public static void AllProductsForAdmin()
        {
            Console.Clear();
            Console.WriteLine("=======================================================================================");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("                                VARDAGSHÖRNAN - KATALOG                                 ");
            Console.ResetColor();
            Console.WriteLine("=======================================================================================");
            Console.WriteLine($"|{"ProduktId",-10} | {"Produkt namn ",-20} | {"Pris",-10} | {"Lager",-4} | {"Leverantör",-15} | {"Utvald",-10}|");
            Console.WriteLine("---------------------------------------------------------------------------------------");
            // skriva ut varje produkt
            using (var db = new MyDbContext())
            {
                foreach (var p in db.Products)
                {
                    string status = p.IsFeatured ? "Utvald" : "Ej utvald";
                    Console.WriteLine($"|{p.Id,-10} | {p.Name,-20} | {p.Price,-10} | {p.Stock,-4} | {p.Supplier,-15} | {status,-10} |");
                }
                // fixa Id mäste inte vara alltid 0
                Console.WriteLine("---------------------------------------------------------------------------------------");

                // WindowHelper.CatalogWindow();
                //Console.SetCursorPosition(44, 32);
                //string searchedProduct = Console.ReadLine();
                //// fixa sökfältet
            }
            Console.WriteLine("Ange Produkt Id för mer detaljer :");
            int IdToSee = int.Parse(Console.ReadLine());
            Console.Clear();
            using (var db = new MyDbContext())
            {
                var productToSee = (from p in db.Products
                                     where p.Id == IdToSee
                                     select p).SingleOrDefault();
                string status = productToSee.IsFeatured ? "Utvald" : "Ej utvald";
                Console.ForegroundColor= ConsoleColor.Green;
                Console.WriteLine("Produkt "+ productToSee.Name + " Detaljer :");
                Console.ResetColor();
                Console.WriteLine("===============================================================================================================================");
                Console.WriteLine($" ProduktId   : {productToSee.Id}");
                Console.WriteLine($" Namn        : {productToSee.Name}");
                Console.WriteLine($" Pris        : {productToSee.Price}");
                Console.WriteLine($" I lager     : {productToSee.Stock}");
                Console.WriteLine($" Leverantör  : {productToSee.Supplier}");
                Console.WriteLine($" Status      : {status}");
                Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------------");
                Console.WriteLine(" Beskrivning:");
                Console.WriteLine(" -----------");
                Console.WriteLine($" {productToSee.Description}");
                Console.WriteLine("===============================================================================================================================");
                Console.WriteLine("Tryck 0 för att gå tillbaka.");
                Console.ReadKey();
                ChangeProductsMenu();
            }
        } // apposto
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
            decimal price = decimal.Parse(Console.ReadLine());
            Console.WriteLine("Kategori ID: ");
            int categoryId = int.Parse(Console.ReadLine());
            Console.WriteLine("Vill du att produkten ska på start sidan?");
            bool featured = bool.Parse(Console.ReadLine());
            Console.WriteLine("Hur mänga på lager?");
            int stock = int.Parse(Console.ReadLine());
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
                db.Products.Add(newProduct);
                db.SaveChanges();
            }
            Console.WriteLine("Produkten har lagts till i webbshoppen.");
            Console.ReadKey();
        } // apposto

        public static void ChangeDetails()
        {
            while (true)
            {
                Console.Clear();
                Helpers.WelcomeTextWindow();
                AllProductsForAdmin();
                Console.SetCursorPosition(1, 36);
                Console.WriteLine("Vilken produkt vill du ändra detaljer på? Ange Produkt Id : ");
                if (!int.TryParse(Console.ReadLine(), out int ChosenId))
                {
                    Console.WriteLine("Fel Id! Försök igen.");
                    Thread.Sleep(1000);
                    continue; // börja om while loopen
                }
                List<string> topText4 = new List<string> { "1. Namn", "2.Beskrivning ", "3. Pris ", "4. ProduktKategori", "5. Leverantör", "6. Lager" };
                var windowTop4 = new Window("Vad vill du ändra? : ", 52, 43, topText4);
                windowTop4.Draw();
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
                            Console.WriteLine("1. Tröjor");
                            Console.WriteLine("2. Byxor");
                            Console.WriteLine("3. Jackor");
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
                            break;
                    }
                    db.SaveChanges();
                    Console.WriteLine("Uppdateringen lyckades.”");

                }
                Console.WriteLine("Tryck på valfri tangent för att fortsätta...");
                Console.ReadKey();
            }

        } // to test

        public static void DeleteAProduct()
        {

            Console.Clear();
            AllProductsForAdmin();
            Console.WriteLine("Ange vilken Id Produkt vill du ta bort: ");
            int IdToCancel = int.Parse(Console.ReadLine());
            using (var db = new MyDbContext())
            {
                var deleteProduct = (from t in db.Products
                                     where t.Id == IdToCancel
                                     select t).SingleOrDefault();
                if (IdToCancel != null)
                {
                    db.Products.Remove(deleteProduct);
                    db.SaveChanges();
                }
                else
                {
                    Console.WriteLine("Fel Id, Försök igen!");
                    return;
                }
            }
            Console.ReadKey();
        } // apposto

    }
}

