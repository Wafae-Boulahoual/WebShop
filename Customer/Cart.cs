using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VardagshörnanApp.Models;

namespace VardagshörnanApp.Customer
{
    internal class Cart
    {
        public static List<OrderItem> cart = new List<OrderItem>();

        public static decimal ShowCart(List<OrderItem>Cart)
        {
            Console.Clear();
            Console.WriteLine("Din varukorg: ");
            Console.WriteLine("---------");

            decimal subTotal = 0;
            if (cart.Count == 0)
            {
                Console.WriteLine("Varukorgen är tom.");
                return 0;
            }
            else
            {
                foreach (var item in cart)
                {
                    decimal itemTotal = item.Price * item.Quantity;
                    Console.WriteLine(item.Quantity + " St " + item.Product.Name + " | Pris: " + item.Price + "Kr ");
                    subTotal += itemTotal;
                }
            }
            Console.WriteLine("-------------------------------");
            Console.WriteLine("Totalt före moms & frakt: " + subTotal + " kr");
            return subTotal;
        }
        public static void UpdateCart()
        { 
            Console.WriteLine("\n U. Uppdatera antal \n C. Checka ut \n Q. Tillbaka");
            if (char.TryParse(Console.ReadLine(), out char choice))
            {
                switch (char.ToLower(choice))
                {
                    case 'u': UpdateQuantity(cart);break;
                    case 'c':CheckOut.Checkout(cart); break;
                    case 'q': return;
                }
            }
            else
            {
                Console.WriteLine("Fel val, försök igen.");
            }
            Console.ReadKey();
        }
        public static void UpdateQuantity(List<OrderItem>cart)
        {
            Console.WriteLine("Vilken produkt vill du ändra kvantitet? Ange produkt namn:");
            string input = Console.ReadLine();

            var item = cart.FirstOrDefault(i =>
                        i.Product.Name.ToLower().Contains(input.ToLower()));

            if(item == null)
            {
                Console.WriteLine("Produkten hittades inte i varukorgen.");
                return;
            }

            Console.WriteLine("Tryck 'A' för att öka kvantitet.");
            Console.WriteLine("Tryck 'M' för att minska kvantitet.");

            char choice = char.ToLower(Console.ReadKey().KeyChar);
            if (choice == 'a')
            {
                if (item.Quantity < item.Product.Stock)
                {
                    item.Quantity++;
                    Console.WriteLine("Kvantitet uppdaterad: " + item.Quantity + " st " + item.Product.Name);
                }
                else
                {
                    Console.WriteLine("Kan inte öka mer än tillgängligt lager!");
                }
            }
            else if(choice == 'm')
            {
                if (item.Quantity > 1)
                {
                    item.Quantity--;
                    Console.WriteLine("Kvantitet updaterad. ");
                }
                else if (item.Quantity == 1)
                {
                    Console.WriteLine("Vill du ta bort produkten från din varukorg? j/n ");
                    char answer = char.ToLower(Console.ReadKey().KeyChar);
                    if(answer == 'j')
                    {
                        cart.Remove(item);
                    }
                }
            }
            CartWindow();
        }
        public static void CartWindow()
        {
            List<string> cartText = new List<string>();
            if (cart.Count == 0)
            {
                cartText.Add("Varukorgen är tom.");
            }
            else
            {
                foreach (OrderItem item in cart)
                {
                    string text =item.Quantity+" st "+item.Product.Name+" - " + item.Price * item.Quantity + " kr";

                    cartText.Add(text);
                }
            }

            var window = new Window("Varukorg", 100, 25, cartText);
            window.Draw();
        }
        public static void AddItemToCart(Product product)
        {
            // ta produkten från varukorgen
            var existItem = cart.FirstOrDefault(p => p.ProductId == product.Id);

            if (existItem != null)
            {
                // Kontrollera lager först sen öka kvantitet
                if (existItem.Quantity < product.Stock)
                {
                    existItem.Quantity++;
                    Console.WriteLine(existItem.Product.Name + " kvantitet ökad: " + existItem.Quantity);
                }
                else
                {
                    Console.WriteLine("Kan inte lägga till fler än tillgängligt lager!");
                }
            }
            else
            {
                // Om slut i lager
                if (product.Stock < 1)
                {
                    Console.WriteLine("Produkten är slut i lager!");
                    return;
                }

                var item = new OrderItem
                {
                    ProductId = product.Id,
                    Product = product,
                    Quantity = 1,
                    Price = product.Price
                };

                cart.Add(item);
                Console.WriteLine(product.Name + " har lagts till i varukorgen!");
            }

            // update summan
            decimal total = cart.Sum(o => o.Price * o.Quantity);
            Console.WriteLine("Totalt i varukorgen: " + total + " kr");
        }
    }
}
