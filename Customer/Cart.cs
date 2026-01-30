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
        public static  decimal ShowCart(List<OrderItem>Cart)
        {
            Console.Clear();
            Console.WriteLine("Din varukorg: ");
            Console.WriteLine("---------");

            decimal subTotal = 0;
            if (cart.Count == 0)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                List<string> topText1 = new List<string> { "", "      Din varukorg är tom!       ", "" };
                var window = new Window("", 50, 15, topText1);
                window.Draw();
                Thread.Sleep(1000);
                Console.ResetColor();
                return 0;
            }
            else
            {
                foreach (var item in cart)
                {
                    decimal itemTotal = item.Price * item.Quantity;
                    Console.WriteLine("Produkt Id : " + item.ProductId + "| " + item.Quantity + " St " + item.Product.Name + " | Pris: " + item.Price + "Kr ");
                    subTotal += itemTotal;
                }
            }
            Console.WriteLine("-------------------------------");
            Console.WriteLine("Totalt före moms & frakt: " + subTotal + " kr");
            return subTotal;
        }
        public static void UpdateCart()
        {
            Console.WriteLine("U. Uppdatera antal");
            Console.WriteLine("C. Checka ut ");
            Console.WriteLine("Q. Tillbaka");
            char choice = Console.ReadKey().KeyChar;
            Console.WriteLine();
            switch (char.ToLower(choice))
                {
                case 'u': UpdateQuantity(cart);break;
                case 'c': CheckOut.Checkout(cart); break;
                case 'q': return;
                default:
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Fel val, försök igen.");
                        Console.ResetColor();
                        break;
                    }
            }
            Console.ReadKey();
        }
        public static void UpdateQuantity(List<OrderItem> cart)
        {
            Console.WriteLine("Ange produktens ID du vill ändra kvantitet på:");

            if (!int.TryParse(Console.ReadLine(), out int productId))
            {
                Console.ForegroundColor= ConsoleColor.Red;
                Console.WriteLine("Felaktigt ID!");
                Console.ResetColor();
                return;
            }

            var item = cart.FirstOrDefault(i => i.ProductId == productId);

            if (item == null)
            {
                Console.ForegroundColor= ConsoleColor.Red;
                Console.WriteLine("Produkten finns inte i varukorgen.");
                Console.ResetColor();
                return;
            }

            Console.WriteLine("Vald produkt: " + item.Product.Name);
            Console.WriteLine("Tryck 'O' för att öka kvantitet.");
            Console.WriteLine("Tryck 'M' för att minska kvantitet.");

            char choice = char.ToLower(Console.ReadKey().KeyChar);
            Console.WriteLine();

            if (choice == 'o')
            {
                if (item.Quantity < item.Product.Stock)
                {
                    item.Quantity++;
                    Console.WriteLine("Kvantitet uppdaterad: " + item.Quantity + " st " + item.Product.Name);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Kan inte öka mer än tillgängligt lager!");
                    Console.ResetColor();
                }
            }
            else if (choice == 'm')
            {
                if (item.Quantity > 1)
                {
                    item.Quantity--;
                    Console.WriteLine("Kvantitet minskad.");
                }
                else
                {
                    Console.WriteLine("Vill du ta bort produkten från varukorgen? j/n");
                    char answer = char.ToLower(Console.ReadKey().KeyChar);

                    if (answer == 'j')
                    {
                        cart.Remove(item);
                        Console.WriteLine("Produkten borttagen från varukorgen.");
                    }
                }
            }
            else
            {
                Console.ForegroundColor= ConsoleColor.Red;
                Console.WriteLine("Ogiltigt val.");
                Console.ResetColor();
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
                    string text ="Produkt Id : " + item.ProductId + " - " + item.Quantity+" st " + item.Product.Name+" - " + item.Price * item.Quantity + " kr";

                    cartText.Add(text);
                }
            }

            var window = new Window("Varukorg", 80, 25, cartText);
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
                    Console.ForegroundColor= ConsoleColor.Red;
                    Console.WriteLine("Kan inte lägga till fler än tillgängligt lager!");
                    Console.ResetColor();
                }
            }
            else
            {
                // Om slut i lager
                if (product.Stock < 1)
                {
                    Console.ForegroundColor=ConsoleColor.Red;
                    Console.WriteLine("Produkten är slut i lager!");
                    Console.ResetColor();
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
                //Console.ForegroundColor = ConsoleColor.Green;
                //Console.WriteLine(product.Name + " har lagts till i varukorgen!");
                //Console.ResetColor();
            }

            // update summan
            decimal total = cart.Sum(o => o.Price * o.Quantity);
            Console.WriteLine("Totalt i varukorgen: " + total + " kr");
        }
    }
}
