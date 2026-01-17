using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VardagshörnanApp.Admin;
using VardagshörnanApp.Models;

namespace VardagshörnanApp.Customer
{
    internal class RegisterCustomer
    {
        public static void RegisterOrLoginWindow()
        {
            List<string> topText10 = new List<string> { "R. registrera dig", "L. Logga in" };
            var windowTop10 = new Window("Register/Login", 110, 1, topText10);
            windowTop10.Draw();
        }
        public static void RegisterNewCustomer()
        {
            Console.Clear();
            //Common.WelcomeTextWindow();
            using (var db = new MyDbContext())
            {
                var customer = new Models.Customer();

                Console.WriteLine("Vänligen fyll i dina uppgifter");
                Console.Write("Förnamn: ");
                customer.FirstName = Console.ReadLine();
                Console.Write("Efternamn: ");
                customer.LastName = Console.ReadLine();
                Console.Write("Adress: ");
                customer.Address = Console.ReadLine();
                Console.Write("Stad: ");
                customer.City = Console.ReadLine();
                Console.Write("Land: ");
                customer.Country = Console.ReadLine();
                Console.Write("E-post: ");
                customer.EmailAdress = Console.ReadLine();
                Console.Write("Telefonnummer (valfri): ");
                customer.PhoneNumber = Console.ReadLine();
                Console.Write("Födelsedatum (yyyy-mm-dd): ");
                DateOnly birthDate;
                while (!DateOnly.TryParse(Console.ReadLine(), out birthDate))
                {
                    Console.Write("Ogiltigt format. Försök igen (yyyy-mm-dd): ");
                }
                customer.BirthDate = birthDate;
                string password;
                string confirmPassword;
                do
                {
                    Console.Write("Skapa ett lösenord: ");
                    password = Console.ReadLine();
                    Console.Write("Bekräfta lösenordet: ");
                    confirmPassword = Console.ReadLine();
                    if (password != confirmPassword)
                    {
                        Console.WriteLine("Lösenorden matchar inte. Försök igen.");
                        Console.WriteLine();
                    }
                }
                while (password != confirmPassword);
                customer.Password = password;

                db.Customers.Add(customer);
                db.SaveChanges();
                Console.WriteLine("Grattis, "+customer.FirstName+"! Din registrering är klar.");
                Console.WriteLine("Användarnamn: "+customer.EmailAdress);
            }
            Console.ReadKey();
        }
        public static void CustomerLogin()
        {
            while (true)
            {
                Console.Clear();
                using (var db = new MyDbContext())
                {
                    Console.Write("Användarnamn (E-post): ");
                    string username = Console.ReadLine()!;

                    Console.Write("Lösenord: ");
                    string password = Console.ReadLine()!;

                    var customer = db.Customers
                                     .FirstOrDefault(c => c.EmailAdress == username && c.Password == password);

                    if (customer != null)
                    {
                        Console.WriteLine($"Välkommen tillbaka, {customer.FirstName} {customer.LastName}!");
                        Thread.Sleep(1500);
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Fel användarnamn eller lösenord.");
                        Console.Write("Vill du försöka igen? (j/n): ");
                        string answer = Console.ReadLine()!.ToLower();
                        if (answer != "j")
                        {
                            Console.WriteLine("Login avbruten.");
                            Thread.Sleep(1500);
                            break;
                        }
                        Console.WriteLine();
                    }
                }
                Console.ReadKey();
            }           
        }       
    }
}
