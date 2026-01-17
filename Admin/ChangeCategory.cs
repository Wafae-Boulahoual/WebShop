using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VardagshörnanApp.Models;

namespace VardagshörnanApp.Admin
{
    internal class ChangeCategory
    {
        public static void AllCategoriesForAdmin()
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("-----------------------------------------------------------------------------------------");
                Console.WriteLine($"| {"Id",-5} | {"Kategori namn",-26} | {"Beskrivning",-48} |");
                Console.WriteLine("-----------------------------------------------------------------------------------------");
                Console.ResetColor();
                using (var db = new MyDbContext())
                {
                    foreach (var category in db.Categories)
                    {
                        Console.WriteLine($"| {category.Id,-5} | {category.Name,-26} | {category.Description,-48} |");
                        Console.WriteLine("-----------------------------------------------------------------------------------------");
                    }
                }
                Console.WriteLine("Tryck en valfri tangent för att fortsätta...");
                Console.ReadKey();
                return;
            }
        }
        public static void AddCategory()
        {
            Console.Clear();
            Console.WriteLine("Lägga till en ny kategori");
            Console.WriteLine("------------------------");
            Console.WriteLine("Kategori namn:");
            string name = Console.ReadLine();
            Console.WriteLine("Detaljerad information :");
            string detail = Console.ReadLine();
            using (var db = new MyDbContext())
            {
                var newCategory = new Category
                {
                    Name = name,
                    Description = detail
                };
                db.Categories.Add(newCategory);
                db.SaveChanges();
            }
            Console.WriteLine("Kategorin har lagts till i webbshoppen.");
            Console.ReadKey();
        }
        public static void ChangeNameCategory()
        {
            Console.Clear();
            AllCategoriesForAdmin();
            Console.WriteLine("Vilken kategori vill du ändra namnet på?");
            Console.WriteLine("Ange Kategori ID: ");
            int choiceId = int.Parse(Console.ReadLine());
            using (var db = new MyDbContext())
            {
                var CategoryToUpdate = (from c in db.Categories
                                        where c.Id == choiceId
                                        select c).SingleOrDefault();
                if (CategoryToUpdate == null)
                {
                    Console.WriteLine("Kategorin hittades inte!");
                    return;
                }
                else
                {
                    Console.WriteLine("Ange den nya namnet:");
                    string NewName = Console.ReadLine();
                    CategoryToUpdate.Name = NewName;
                    db.SaveChanges();
                    Console.WriteLine("Uppdateringen lyckades.");
                }
                Console.ReadKey();
            }
        }
        public static void ChangeDescriptionCategory()
        {
            Console.Clear();
            AllCategoriesForAdmin();
            Console.WriteLine("Vilken kategori vill du ändra beskrivning på?");
            Console.WriteLine("Ange Kategori ID: ");
            int Id = int.Parse(Console.ReadLine());
            using (var db = new MyDbContext())
            {
                var CategoryToUpdate = (from c in db.Categories
                                        where c.Id == Id
                                        select c).SingleOrDefault();
                if (CategoryToUpdate == null)
                {
                    Console.WriteLine("Kategorin hittades inte!");
                    return;
                }
                else
                {
                    Console.WriteLine("Ange den nya beskrivningen:");
                    string NewDescription = Console.ReadLine();
                    CategoryToUpdate.Description = NewDescription;
                    db.SaveChanges();
                    Console.WriteLine("Uppdateringen lyckades.");
                }
            }
            Console.ReadKey();
        }
        public static void ChangeCategoryWindow()
        {
            List<string> topText5 = new List<string> { "","A. Se alla kategorier", "B. Lägga till en ny kategori", "C. Ändra en kategori namn", "D. Ändra en kategori beskrivning","" };
            Console.ForegroundColor = ConsoleColor.Green;
            var windowTop5 = new Window("Hantera en kategori:", 40, 15, topText5);
            Console.ResetColor();
            windowTop5.Draw();
        }
    }
}
