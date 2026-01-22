using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VardagshörnanApp.Models;

namespace VardagshörnanApp.Customer
{
    internal class CheckOut
    {

        public static void Checkout(List<OrderItem> cart)
        {
            if (cart.Count == 0)
            {
                Console.WriteLine("Varukorgen är tom.");
                return;
            }

            Console.Clear();
            Console.WriteLine("CHECKOUT");
            Console.WriteLine("--------");

            // Mostra carrello e calcola subtotal + IVA
            decimal subTotal = Cart.ShowCart(cart);
            decimal vat = subTotal * Order.VAT;
            Console.WriteLine($"Moms (25%): {vat} kr");

            // MODIFICA: controllo stock prima del checkout
            foreach (var item in cart)
            {
                if (item.Quantity > item.Product.Stock)
                {
                    Console.WriteLine($"Fel: {item.Product.Name} har bara {item.Product.Stock} i lager.");
                    return;
                }
            }

            // Scelta spedizione
            decimal shippingCost = 0;
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
                    Console.WriteLine("Fel val, försök igen.");
                }
            }

            // Scelta metodo di pagamento
            PaymentMethod paymentMethod;
            while (true) // MODIFICA: loop finché input non valido
            {
                Console.WriteLine("Välj betalningsmetod:");
                Console.WriteLine("1. Kort");
                Console.WriteLine("2. Faktura");
                Console.WriteLine("3. Swish");

                char paymentChoice = Console.ReadKey().KeyChar;
                Console.WriteLine();

                if (paymentChoice == '1') { paymentMethod = PaymentMethod.Card; break; }
                else if (paymentChoice == '2') { paymentMethod = PaymentMethod.Invoice; break; }
                else if (paymentChoice == '3') { paymentMethod = PaymentMethod.Swish; break; }
                else { Console.WriteLine("Fel val, försök igen."); }
            }

            decimal total = subTotal + vat + shippingCost;
            Console.WriteLine("Totalt att betala: " + total + " kr");
            Console.WriteLine("Vill du bekräfta beställningen? (j/n)");

            char confirm = char.ToLower(Console.ReadKey().KeyChar);
            Console.WriteLine();

            if (confirm != 'j')
            {
                Console.WriteLine("Beställningen avbröts.");
                return;
            }

            // Login / registrazione
            Models.Customer customer = RegisterCustomer.HandleLoginOrRegister();
            if (customer == null)
            {
                Console.WriteLine("Ingen kund inloggad.");
                return;
            }

            // Salva ordine
            SaveOrder(customer, cart, shippingMethod, shippingCost, paymentMethod);

            // MODIFICA: aggiornamento stock prodotti
            using (var db = new MyDbContext())
            {
                foreach (var item in cart)
                {
                    var product = db.Products.FirstOrDefault(p => p.Id == item.ProductId);
                    if (product != null)
                    {
                        product.Stock -= item.Quantity;
                    }
                }
                db.SaveChanges();
            }

            Console.WriteLine("Beställningen är genomförd!");
            cart.Clear();
        }

        public static void SaveOrder(Models.Customer customer, List<OrderItem> cart,
                              ShippingMethod shippingMethod, decimal shippingCost,
                              PaymentMethod paymentMethod)
        {
            using (var db = new MyDbContext())
            {
                // Calcolo totale dell'ordine (subtotal + IVA + spedizione)
                decimal subTotal = cart.Sum(ci => ci.Price * ci.Quantity);
                decimal vat = subTotal * Order.VAT;
                decimal totalAmount = subTotal + vat + shippingCost; // MODIFICA: totale finale

                var order = new Order
                {
                    CustomerId = customer.Id,
                    OrderDate = DateTime.Now,
                    ShippingMethod = shippingMethod,
                    ShippingCost = shippingCost,
                    PaymentMethod = paymentMethod,
                    TotalAmount = totalAmount,
                    Status = OrderStatus.Betald,
                    OrderItems = cart.Select(ci => new OrderItem
                    {
                        ProductId = ci.ProductId,
                        Quantity = ci.Quantity,
                        Price = ci.Price
                    }).ToList()
                };

                db.Orders.Add(order);
                db.SaveChanges();
            }
        }


    }
}