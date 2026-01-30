using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VardagshörnanApp.Admin;
using VardagshörnanApp.Customer;
using VardagshörnanApp.Models;

namespace VardagshörnanApp
{
    internal class Common // innehåller metoder som är gemensamma för både admin och kund
    {
        public static void WelcomeTextWindow()
        {
            List<string> topText = new List<string> { "                             * VardagsHörnan *", "                        ---------------------------", "               Små detaljer som gör hem och kontor mer trivsamma.                " };
            var windowTop = new Window("", 30, 5, topText);
            windowTop.Draw();
        }
        public static void WelcomeUser(Administrator? administrator, Models.Customer? customer)
         // Om administrator är null används metoden för kunden, och vice versa.
        {
            string name;
            if (administrator != null)
            {
                name = administrator.UserName;
            }
            else if (customer != null)
            {
                name = customer.FirstName;
            }
            else
            {
                name = "Gäst"; 
            }
            List<string> topText = new List<string> {"Välkommen "+name};
            var windowTop = new Window("", 60, 1, topText);
            windowTop.Draw();
        }
        public static async Task CustomerOrAdminAsync()
        // Async eftersom metoden anropar asynkrona metoder 
        {
            while (true)
            {
                Console.Clear();
                WelcomeTextWindow();
                List<string> topText2 = new List<string> { "                ", "   Tryck K Om du är en Kund   ", "   Tryck A om du är en Admin   ", "                  " };
                var windowTop2 = new Window("Välkommen till shoppen!", 52, 15, topText2);
                windowTop2.Draw();
                
                char role = char.ToLower(Console.ReadKey().KeyChar);
                if (role == 'k')
                {
                    await CustomerPage.CustomerMenuAsync(); // går till kund sidan
                }
                else if (role == 'a')
                {
                    var admin = await AdminPage.AdminCheckLoginAsync();
                    if (admin != null) // om admin kan logga in
                    {
                        AdminPage.AdminMenu(); // går till admin sidan
                    }
                    else 
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Fel inloggning! Försök igen.");
                        Console.ResetColor();
                        await Task.Delay(1000);
                        continue; 
                    }
                }
                else
                {
                    Console.WriteLine("Fel val! Försök igen");
                    continue; 
                }
                Console.ReadKey();
            }
        }
        public static void CategorySeeder()
        {
            //using (var db = new MyDbContext())
            //{
            //    var categories = new List<Category>
            //    {
            //        new Category { Name = "Hem & Dekoration", Description = "Produkter för hem och vardag" },
            //        new Category { Name = "Kök & Tillbehör", Description = "Köksartiklar och tillbehör" },
            //        new Category { Name = "Böcker & Kontorsmaterial", Description = "Böcker, pennor och skrivmaterial" }
            //    };
            //    db.Categories.AddRange(categories);
            //    db.SaveChanges();
            //}
        }
        public static void AllProductsTable(int categoryId = 0) 
            // Visar alla produkter, eller produkter i en viss kategori om categoryId är > 0
        {
            Console.Clear();
            Console.WriteLine("======================================================================================================================");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("                                              VARDAGSHÖRNAN - KATALOG                                                  ");
            Console.ResetColor();
            Console.WriteLine("=======================================================================================================================");
            Console.WriteLine($"|{"ProduktId",-10} | {"Produkt namn",-20} | {"Pris",-10} | {"Lagersaldo",-15} | {"Leverantör",-15} | {"Kategori",-28}");
            Console.WriteLine("-----------------------------------------------------------------------------------------------------------------------");

            using (var db = new MyDbContext())
            {
                var products = db.Products
                               .Include(p => p.Category).ToList(); // för att visa kategori namnet

                if (categoryId >0)
                {
                    products = products.Where(p => p.CategoryId == categoryId).ToList();
                }
                foreach (var p in products)
                {
                    string status;
                    if (p.Stock == 0)
                        status = "Slut i lager";
                    else if (p.Stock < 5)
                        status = "Få kvar i lager";
                    else
                        status = "Tillgänglig";
                    Console.WriteLine($"|{p.Id,-10} | {p.Name,-20} | {p.Price,-10} | {status,-15} | {p.Supplier,-15} |  {p.Category.Name,-28} ");
                }
            }
            Console.WriteLine("-----------------------------------------------------------------------------------------------------------------------");
        } 
        public static void ProductSeeder()
        {
            //using (var db = new MyDbContext())
            //{
            //    var products = new List<Product>
            //    {
            //         Hem & Dekoration
            //        new Product { Name = "Doftljus", Description = "Ett mjukt vaniljdoftande ljus som sprider en lugn och behaglig atmosfär i hemmet. Perfekt för vardagsrum eller sovrum.", Price = 120, Stock = 10, CategoryId = 1, IsFeatured = true, Supplier = "NordicHome" },
            //        new Product { Name = "Keramikvas", Description = "Handgjord dekorativ vas i keramik med unik design. Passar för färska blommor eller som en elegant inredningsdetalj.", Price = 250, Stock = 5, CategoryId = 1, IsFeatured = false, Supplier = "Ceramica AB" },
            //        new Product { Name = "Träram", Description = "Naturlig fotoram i trä som ger en varm och stilren känsla. Perfekt för familjebilder eller konsttryck.", Price = 150, Stock = 15, CategoryId = 1, IsFeatured = false, Supplier = "WoodArt" },
            //        new Product { Name = "Dekorativ kudde", Description = "Färgglad kudde i mjukt bomullstyg som ger komfort och stil till soffan eller sängen. Lätt att matcha med andra textilier.", Price = 180, Stock = 8, CategoryId = 1, IsFeatured = false, Supplier = "SoftHome" },
            //        new Product { Name = "Doftspray för hemmet", Description = "Lavendeldoftande spray som fräschar upp alla rum. Skapar en avslappnande miljö och passar perfekt för gästrum eller vardagsrum.", Price = 200, Stock = 12, CategoryId = 1, IsFeatured = false, Supplier = "AromaNord" },

            //         Kök & Tillbehör
            //        new Product { Name = "Kaffemugg", Description = "Vit keramisk mugg på 250ml, perfekt för kaffe, te eller varm choklad. Enkel att rengöra och tålig i diskmaskin.", Price = 80, Stock = 20, CategoryId = 2, IsFeatured = false, Supplier = "NordicHome" },
            //        new Product { Name = "Termosflaska", Description = "Rostfri stålflaska på 500ml som håller drycken varm eller kall i timmar. Perfekt för resor, jobbet eller fritidsaktiviteter.", Price = 220, Stock = 10, CategoryId = 2, IsFeatured = false, Supplier = "ThermoPlus" },
            //        new Product { Name = "Moka kaffebryggare", Description = "Traditionell moka kaffebryggare för 6 koppar. Brygger kaffe med rik arom och robust smak, idealisk för morgon eller fika.", Price = 300, Stock = 7, CategoryId = 2, IsFeatured = true, Supplier = "CoffeeTech" },
            //        new Product { Name = "Skärbräda i bambu", Description = "Hållbar och miljövänlig skärbräda i bambu. Stabil och tålig yta för att hacka grönsaker, kött eller bröd utan att skada knivarna.", Price = 150, Stock = 15, CategoryId = 2, IsFeatured = false, Supplier = "BambooHouse" },
            //        new Product { Name = "Förslutbar glasburk", Description = "Glasburk med tätslutande lock som bevarar matens fräschhet. Perfekt för kryddor, spannmål eller hemmagjorda syltar.", Price = 90, Stock = 18, CategoryId = 2, IsFeatured = false, Supplier = "GlassCo" },

            //         Böcker & Kontorsmaterial
            //        new Product { Name = "Veckoplanerare", Description = "Veckoplanerare för 2026 med tydliga uppdelningar per vecka och plats för anteckningar. Hjälper dig hålla koll på scheman och mål.", Price = 120, Stock = 25, CategoryId = 3, IsFeatured = true, Supplier = "OfficeSupplies" },
            //        new Product { Name = "Anteckningsbok A5", Description = "Rutig anteckningsbok med 100 sidor. Perfekt för skola, jobb eller kreativa idéer, lätt att ta med i väskan.", Price = 50, Stock = 30, CategoryId = 3, IsFeatured = false, Supplier = "PaperCo" },
            //        new Product { Name = "Fyllepenn", Description = "Elegant svart fyllepenn som skriver mjukt och ger en klassisk känsla. Passar för brev, anteckningar eller konstnärligt arbete.", Price = 200, Stock = 10, CategoryId = 3, IsFeatured = false, Supplier = "PenArt" },
            //        new Product { Name = "Kokbok", Description = "Mellanösterns receptsamling med tydliga steg-för-steg-instruktioner. Perfekt för nybörjare och erfarna kockar.", Price = 250, Stock = 12, CategoryId = 3, IsFeatured = false, Supplier = "FoodBooks" },
            //        new Product { Name = "Markeringspennor", Description = "Set med 5 pastellfärgade markeringspennor som är perfekta för studier, planering eller kreativa projekt.", Price = 80, Stock = 20, CategoryId = 3, IsFeatured = false, Supplier = "OfficeSupplies" }
            //    };

            //    db.Products.AddRange(products);
            //    db.SaveChanges();
            //}
        }
        public static void SearchAProductWindow() 
        {
            List<string> topText2 = new List<string> { "Tryck S för att söka en produkt" };
            var windowTop2 = new Window("", 1, 1, topText2);
            windowTop2.Draw();
        }
        public static List<Product> SearchBarre() 
        {
            Console.Clear();
            Console.WriteLine("Vilken produkt söker du?");
            string input = Console.ReadLine();

            using (var db = new MyDbContext())
            {
                // Hämtar alla produkter som innehåller sökordet
                var products = db.Products
                                 .Include(p => p.Category)
                                 .Where(p => p.Name.ToLower().Contains(input.ToLower()))
                                 .ToList();

                if (products.Count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Ingen produkt hittades.");
                    Console.ResetColor();
                    Console.ReadKey();
                }
                return products; //returnerar ändå en lista som kan vara tom om ingen produkt hittades
            }
        }
        
        public static void WaitingWindow()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            List<string> topText = new List<string> { "", "      Vänligen var god vänta...       ", "" };
            var waitingWindow = new Window("", 50, 15, topText);
            waitingWindow.Draw();
            Console.ResetColor();

        }


    }
}
