using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VardagshörnanApp.Admin
{
    internal class AdminPage
    {
        public static void AdminMenu()
        {
            while (true)
            {
                Console.Clear();
                Common.WelcomeTextWindow();
                ChangeProduct.ChangeProductWindow();
                ChangeCategory.ChangeCategoryWindow();
                ChangeCustomer.ChangeCustomerWindow();
                Common.SearchAProductWindow();
                Statistics.StatisticsWindow();
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
                    case 's': ChangeProduct.SearchProductAdmin(); break; // rör inte
                    case 'q': Common.WelcomeTextWindow(); Common.CustomerOrAdmin(); return;
                    default: Console.WriteLine("Fel val! försök igen "); break;
                } 
            }

        }
        public static void LoginAdmin()
        {
            Console.WriteLine("Ange Användarnamn:");
            string username = Console.ReadLine();
            
        }
    }

}


