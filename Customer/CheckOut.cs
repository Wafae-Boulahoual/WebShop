using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VardagshörnanApp.Models;

namespace VardagshörnanApp.Customer
{
    internal class CheckOut
    {
        public static void Checkout(List<OrderItem> cart) // dividera metoden, för lång!
        {
            if (cart.Count == 0)
            {
                Console.WriteLine("Varukorgen är tom.");
                return;
            }

            Console.Clear();
            Console.WriteLine("CHECKOUT");
            Console.WriteLine("--------");

            // visa varukorgen och totalt
            decimal subTotal = Cart.ShowCart(cart);
            decimal vat = subTotal * Order.VAT;
            Console.WriteLine("Moms (25%): " + vat + " kr");

            // kontrollera lager först
            foreach (var item in cart)
            {
                if (item.Quantity > item.Product.Stock)
                {
                    Console.WriteLine("Fel: " + item.Product.Name + " har bara " + item.Product.Stock + " i lager.");
                    return;
                }
            }

            decimal shippingCost = 0; // initialisering
            ShippingMethod shippingMethod;

            while (true)
            {
                Console.WriteLine("Välj fraktalternativ:");
                Console.WriteLine("1. Standard (39 kr)");
                Console.WriteLine("2. Express (69 kr)");

                char shippingChoice = Console.ReadKey().KeyChar;
                Console.WriteLine();

                if (shippingChoice == '1')
                {
                    shippingCost = 39;
                    shippingMethod = ShippingMethod.Standard;
                    break;
                }
                else if (shippingChoice == '2')
                {
                    shippingCost = 69;
                    shippingMethod = ShippingMethod.Express;
                    break;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Fel val, försök igen.");
                    Console.ResetColor();
                }
            }

            PaymentMethod paymentMethod;// val av betalning metod
            while (true)
            {
                Console.WriteLine("Välj betalningsmetod:");
                Console.WriteLine("1. Kort");
                Console.WriteLine("2. Faktura");
                Console.WriteLine("3. Swish");

                char paymentChoice = Console.ReadKey().KeyChar;
                Console.WriteLine();

                if (paymentChoice == '1')
                {
                    paymentMethod = PaymentMethod.Card;
                    break;
                }
                else if (paymentChoice == '2')
                {
                    paymentMethod = PaymentMethod.Invoice;
                    break;
                }
                else if (paymentChoice == '3')
                {
                    paymentMethod = PaymentMethod.Swish;
                    break;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Fel val, försök igen.");
                    Console.ResetColor();
                }
            }

            decimal total = subTotal + vat + shippingCost;
            Console.WriteLine("Totalt att betala: " + total + " kr");
            Console.WriteLine("Vill du bekräfta beställningen? (j/n)");
            char confirm = char.ToLower(Console.ReadKey().KeyChar);
            Console.WriteLine();

            if (confirm != 'j')
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Beställningen avbröts.");
                Console.ResetColor();
                return;
            }

            // Loggning/registrering
            Models.Customer customer = RegisterCustomer.HandleLoginOrRegister();
            if (customer == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ingen kund inloggad.");
                Console.ResetColor();
                return;
            }

            using (var db = new MyDbContext())
            {
                try // försök att spara ordern coh att spara den nya lager i databasen
                {
                    var order = new Order
                    {
                        CustomerId = customer.Id,
                        OrderDate = DateTime.Now,
                        ShippingMethod = shippingMethod,
                        ShippingCost = shippingCost,
                        PaymentMethod = paymentMethod,
                        TotalAmount = total,
                        OrderItems = cart.Select(ci => new OrderItem
                        {
                            ProductId = ci.ProductId,
                            Quantity = ci.Quantity,
                            Price = ci.Price
                        }).ToList()
                    };
                    db.Orders.Add(order);

                    foreach (var item in cart)
                    {
                        var product = db.Products.FirstOrDefault(p => p.Id == item.ProductId);
                        if (product != null)
                        {
                            product.Stock -= item.Quantity;
                        }
                    }
                    db.SaveChanges();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Beställningen är genomförd!");
                    Console.ResetColor();
                    Thread.Sleep(1000);
                    cart.Clear();
                    AfterCheckOut.AfterCheckoutMenu(customer);
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Ett del inträffades!");
                    Console.WriteLine(ex.Message);
                    Console.ResetColor();
                }
            }
        }
    } 
}
