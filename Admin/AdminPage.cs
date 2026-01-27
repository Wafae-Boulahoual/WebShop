using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VardagshörnanApp.Customer;
using VardagshörnanApp.Models;

namespace VardagshörnanApp.Admin
{
    internal class AdminPage
    {
        public static Administrator LoggedInAdministrator;
        public static void AdminMenu()
        {
            while (true)
            {
                Console.Clear();
                Common.WelcomeUser(LoggedInAdministrator, null);
                AdminPageWindows();
                char choice = char.ToLower(Console.ReadKey().KeyChar);
                switch (choice)
                {
                    case '1': ChangeProduct.AllProductsForAdmin(); break;
                    case '2': ChangeProduct.AddProduct(); break;
                    case '3': ChangeProduct.ChangeDetails();  break;
                    case '4': ChangeProduct.DeleteAProduct(); break;
                    case '5': ChangeProduct.ChangeFeaturedProducts(); break;
                    case 'a': ChangeCategory.AllCategoriesForAdmin(); break;
                    case 'b': ChangeCategory.AddCategory(); break;
                    case 'c': ChangeCategory.ChangeNameCategory();break;
                    case 'd': ChangeCategory.ChangeDescriptionCategory(); break;
                    case 'x': ChangeCustomer.AllCustomersForAdmin(); break;
                    case 'y': ChangeCustomer.ChangeCustomersDetails(); break;
                    case 'z': ChangeCustomer.OrdersHistorik(); break;
                    case 'w': Statistics.Queries(); break;
                    case 's': ChangeProduct.SearchProductAdmin(); break; 
                    case 'q': Session.LoggedInAdministrator = null; LoggedInAdministrator = null; return;
                    default: 
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Fel val! försök igen ");
                        Console.ReadKey();
                        Console.ResetColor();
                        break;
                } 
            }

        } // testa sen
        public static void AdminPageWindows()
        {
            Common.WelcomeTextWindow();
            ChangeProduct.ChangeProductWindow();
            ChangeCategory.ChangeCategoryWindow();
            ChangeCustomer.ChangeCustomerWindow();
            Common.SearchAProductWindow();
            Statistics.StatisticsWindow();
        } // apposto
        public static void LoginAdmin()
        {
            //using (var db = new MyDbContext())
            //{
            //    var administrators = new List<Administrator>
            //    {
            //        new Administrator { UserName = "admin85", Password = "123456" }
            //    };

            //    db.Administrators.AddRange(administrators);
            //    db.SaveChanges();
            //}

        }
        public static async Task<Models.Administrator?> AdminCheckLoginAsync()
        {
            using (var db = new MyDbContext())
            {
                Console.Write("Ange användarnamn: ");
                string username = Console.ReadLine();

                Console.Write("Ange lösenord: ");
                string password = Console.ReadLine();

                Console.Clear();
                Common.WaitingWindow();

                var administrator = await db.Administrators
                                    .FirstOrDefaultAsync(a => a.UserName == username && a.Password == password);
                Console.Clear();
                if (administrator == null)
                {
                    Console.ForegroundColor= ConsoleColor.Red;
                    Console.WriteLine("Fel inloggning!");
                    await Task.Delay(1000);
                    Console.ResetColor();
                    return null;
                }

                LoggedInAdministrator = administrator;
                Session.LoggedInAdministrator = administrator;
                Console.ForegroundColor = ConsoleColor.Green;
                List<string> topText = new List<string> { "", "      Inloggning lyckades!      ", "" };
                var waitingWindow = new Window("", 50, 15, topText);
                waitingWindow.Draw();
                Console.ResetColor();
                await Task.Delay(1000);
                return administrator;
            }
        }
       
    }

}


