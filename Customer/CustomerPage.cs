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
        public static void CustomerMenu()
        {
            while (true)
            {
                Console.Clear();
                Common.WelcomeTextWindow();
                RegisterCustomer.RegisterOrLoginWindow();
                Cart.CartWindow();
                Common.SearchAProductWindow();
                //Shop.FeaturedProductsWindows();
                Shop.CustomerWindow();
                Shop.ProductsIncategoriesWindow();
                var featuredProducts = Shop.FeaturedProductsWindows();
                char choice = char.ToLower(Console.ReadKey().KeyChar);
                switch (choice)
                {
                    case 'l': RegisterCustomer.CustomerLogin();break;
                    case 'r': RegisterCustomer.RegisterNewCustomer(); break;
                    case 'x': Shop.ProductDetailsForCustomer(featuredProducts[0]); break;
                    case 'y':Shop.ProductDetailsForCustomer(featuredProducts[1]);break;
                    case 'z':Shop.ProductDetailsForCustomer(featuredProducts[2]); break;
                    case '1':Shop.ProductsForCustomer(1);break;
                    case '2': Shop.ProductsForCustomer(2); break;
                    case '3':Shop.ProductsForCustomer(3); break;
                    case 'a':Shop.ProductsForCustomer(); break;
                    case 'v':Cart.ShowCart(Cart.cart); Cart.UpdateCart();break;
                    case 'q':Common.WelcomeTextWindow(); Common.CustomerOrAdmin();return;
                    case 's':Shop.SearchProductCustomer();break; // rör inte
                    default: Console.WriteLine("Fel val! försök igen");Thread.Sleep(500); break;
                }
            }
        }
    }
}
