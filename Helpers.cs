using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VardagshörnanApp.Admin;
using VardagshörnanApp.Models;

namespace VardagshörnanApp
{
    internal class Helpers
    {
        public static void WelcomeTextWindow()
        {
            List<string> topText = new List<string> { "                  * VardagsHörnan *", "             ---------------------------", "               Din stil, ditt uttryck!                " };
            var windowTop = new Window("", 42, 1, topText);
            windowTop.Draw();

        }
        public static void CustomerOrAdmin()
        {
            while (true)
            {
                Console.Clear();
                WelcomeTextWindow();
                List<string> topText2 = new List<string> { "                ", "   Tryck K Om du är en Kund   ", "   Tryck A om du är en Admin   ", "                  " };
                var windowTop2 = new Window("Välkommen till shoppen!", 52, 10, topText2);
                windowTop2.Draw();

                char role = char.ToLower(Console.ReadKey().KeyChar);
                if (role == 'k')
                {
                    Console.WriteLine("customer page visas");
                    //CustomerMenu(products);
                }
                else if (role == 'a')
                {
                    AdminPage.AdminMenu();
                }
                else
                {
                    Console.WriteLine("Fel val! Försök igen");
                    Console.ReadKey();
                    continue;
                }
                Console.ReadKey();
            }
        }
        public static void CategorySeeder()
        {
            using (var db = new MyDbContext())
            {
                var categories = new List<Category>
                {
                    new Category { Name = "Hem & Dekoration", Description = "Produkter för hem och vardag" },
                    new Category { Name = "Kök & Tillbehör", Description = "Köksartiklar och tillbehör" },
                    new Category { Name = "Böcker & Kontorsmaterial", Description = "Böcker, pennor och skrivmaterial" }
                };
                db.Categories.AddRange(categories);
                db.SaveChanges();
            }
        }
        public static void SearchProduct()
        {
            Console.WriteLine("Ange Produkt Id för mer detaljer :");
            int IdToSee = int.Parse(Console.ReadLine());
            Console.Clear();
            using (var db = new MyDbContext())
            {
                var productToSee = (from p in db.Products
                                    where p.Id == IdToSee
                                    select p).SingleOrDefault();
                string status = productToSee.IsFeatured ? "Utvald" : "Ej utvald";
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Produkt " + productToSee.Name + " Detaljer :");
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
            }
        }
        public static void ProductSeeder()
        {
            using (var db = new MyDbContext())
            {
                var products = new List<Product>
                {
                    // Hem & Dekoration 
                    new Product { Name = "Doftljus", Description = "Ett mjukt vaniljdoftande ljus som sprider en lugn och behaglig atmosfär i hemmet. Perfekt för vardagsrum eller sovrum.", Price = 120, Stock = 10, CategoryId = 1, IsFeatured = true, Supplier = "NordicHome" },
                    new Product { Name = "Keramikvas", Description = "Handgjord dekorativ vas i keramik med unik design. Passar för färska blommor eller som en elegant inredningsdetalj.", Price = 250, Stock = 5, CategoryId = 1, IsFeatured = false, Supplier = "Ceramica AB" },
                    new Product { Name = "Träram", Description = "Naturlig fotoram i trä som ger en varm och stilren känsla. Perfekt för familjebilder eller konsttryck.", Price = 150, Stock = 15, CategoryId = 1, IsFeatured = false, Supplier = "WoodArt" },
                    new Product { Name = "Dekorativ kudde", Description = "Färgglad kudde i mjukt bomullstyg som ger komfort och stil till soffan eller sängen. Lätt att matcha med andra textilier.", Price = 180, Stock = 8, CategoryId = 1, IsFeatured = false, Supplier = "SoftHome" },
                    new Product { Name = "Doftspray för hemmet", Description = "Lavendeldoftande spray som fräschar upp alla rum. Skapar en avslappnande miljö och passar perfekt för gästrum eller vardagsrum.", Price = 200, Stock = 12, CategoryId = 1, IsFeatured = false, Supplier = "AromaNord" },

                    // Kök & Tillbehör 
                    new Product { Name = "Kaffemugg", Description = "Vit keramisk mugg på 250ml, perfekt för kaffe, te eller varm choklad. Enkel att rengöra och tålig i diskmaskin.", Price = 80, Stock = 20, CategoryId = 2, IsFeatured = false, Supplier = "NordicHome" },
                    new Product { Name = "Termosflaska", Description = "Rostfri stålflaska på 500ml som håller drycken varm eller kall i timmar. Perfekt för resor, jobbet eller fritidsaktiviteter.", Price = 220, Stock = 10, CategoryId = 2, IsFeatured = false, Supplier = "ThermoPlus" },
                    new Product { Name = "Moka kaffebryggare", Description = "Traditionell moka kaffebryggare för 6 koppar. Brygger kaffe med rik arom och robust smak, idealisk för morgon eller fika.", Price = 300, Stock = 7, CategoryId = 2, IsFeatured = true, Supplier = "CoffeeTech" },
                    new Product { Name = "Skärbräda i bambu", Description = "Hållbar och miljövänlig skärbräda i bambu. Stabil och tålig yta för att hacka grönsaker, kött eller bröd utan att skada knivarna.", Price = 150, Stock = 15, CategoryId = 2, IsFeatured = false, Supplier = "BambooHouse" },
                    new Product { Name = "Förslutbar glasburk", Description = "Glasburk med tätslutande lock som bevarar matens fräschhet. Perfekt för kryddor, spannmål eller hemmagjorda syltar.", Price = 90, Stock = 18, CategoryId = 2, IsFeatured = false, Supplier = "GlassCo" },

                    // Böcker & Kontorsmaterial 
                    new Product { Name = "Veckoplanerare", Description = "Veckoplanerare för 2026 med tydliga uppdelningar per vecka och plats för anteckningar. Hjälper dig hålla koll på scheman och mål.", Price = 120, Stock = 25, CategoryId = 3, IsFeatured = true, Supplier = "OfficeSupplies" },
                    new Product { Name = "Anteckningsbok A5", Description = "Rutig anteckningsbok med 100 sidor. Perfekt för skola, jobb eller kreativa idéer, lätt att ta med i väskan.", Price = 50, Stock = 30, CategoryId = 3, IsFeatured = false, Supplier = "PaperCo" },
                    new Product { Name = "Fyllepenn", Description = "Elegant svart fyllepenn som skriver mjukt och ger en klassisk känsla. Passar för brev, anteckningar eller konstnärligt arbete.", Price = 200, Stock = 10, CategoryId = 3, IsFeatured = false, Supplier = "PenArt" },
                    new Product { Name = "Kokbok", Description = "Mellanösterns receptsamling med tydliga steg-för-steg-instruktioner. Perfekt för nybörjare och erfarna kockar.", Price = 250, Stock = 12, CategoryId = 3, IsFeatured = false, Supplier = "FoodBooks" },
                    new Product { Name = "Markeringspennor", Description = "Set med 5 pastellfärgade markeringspennor som är perfekta för studier, planering eller kreativa projekt.", Price = 80, Stock = 20, CategoryId = 3, IsFeatured = false, Supplier = "OfficeSupplies" }
                };

                db.Products.AddRange(products);
                db.SaveChanges();
            }
        }
    }
}
