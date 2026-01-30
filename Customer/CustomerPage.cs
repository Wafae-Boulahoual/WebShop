using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VardagshörnanApp.Admin;
using VardagshörnanApp.Models;

namespace VardagshörnanApp.Customer
{
    internal class CustomerPage
    {
        public static async Task CustomerMenuAsync()
        {
            while (true)
            {
                Console.Clear();
                Common.WaitingWindow();
                var featuredProducts = await Shop.TakeFeaturedProductsAsync();
                Console.Clear();
                Common.WelcomeUser(null, Session.LoggedInCustomer);
                Common.WelcomeTextWindow();
                RegisterCustomer.RegisterOrLoginWindow();
                Cart.CartWindow();
                Common.SearchAProductWindow();
                Shop.CustomerWindow();
                Shop.ProductsIncategoriesWindow();
                Shop.FeaturedProductsWindows(featuredProducts);
                char choice = char.ToLower(Console.ReadKey().KeyChar);
                switch (choice)
                {
                    case 'l':Session.LoggedInCustomer = RegisterCustomer.CustomerLogin(); break;
                    case 'r':Session.LoggedInCustomer = RegisterCustomer.RegisterNewCustomer(); break;
                    case 'x':Shop.ProductDetailsForCustomer(featuredProducts[0]); break;
                    case 'y':Shop.ProductDetailsForCustomer(featuredProducts[1]); break;
                    case 'z':Shop.ProductDetailsForCustomer(featuredProducts[2]); break;
                    case '1':Shop.ProductsForCustomer(1); break;
                    case '2':Shop.ProductsForCustomer(2); break;
                    case '3':Shop.ProductsForCustomer(3); break;
                    case 'a':Shop.ProductsForCustomer(); break;
                    case 'v':Cart.ShowCart(Cart.cart); Cart.UpdateCart(); break;
                    case 'q':Session.LoggedInCustomer = null; return;
                    case 's':Shop.SearchProductCustomer(); break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Fel val! försök igen");
                        Console.ResetColor(); Thread.Sleep(500); 
                        break;
                }
            }
        }
        public static void CustomerPageWindows()
        {

        }
        
    }
}
