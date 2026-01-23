using Microsoft.EntityFrameworkCore;
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
            List<string> topText = new List<string> { "R. registrera dig", "L. Logga in" };
            var windowTop = new Window("Register/Login", 110, 1, topText);
            windowTop.Draw();
        } // apposto
        public static Models.Customer RegisterNewCustomer()
        {
            Console.Clear();
            Common.WelcomeTextWindow();
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
                bool emailExists;
                do
                {
                    Console.Write("E-post: ");
                    customer.EmailAdress = Console.ReadLine();

                    // Kontrollera om mejlet redan finns
                    var existingCustomer = db.Customers
                                             .FirstOrDefault(c => c.EmailAdress == customer.EmailAdress);

                    emailExists = existingCustomer != null; // om mejlet finns redan

                    if (emailExists)
                    {
                        Console.WriteLine("Denna e-post är redan registrerad, försök en annan.");
                    }

                } 
                while (emailExists);

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

                try // försök att spara kunden i databasen
                {
                    db.Customers.Add(customer);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Registrering misslyckades: " + ex.Message);
                }
                
                Session.LoggedInCustomer = customer;
                Console.WriteLine("Grattis, " + customer.FirstName + "! Din registrering är klar.");
                Console.WriteLine("Användarnamn: " + customer.EmailAdress);
                Console.ReadKey();
                return customer;
            }
        }
        public static Models.Customer CustomerLogin()
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

                if (customer == null)
                {
                    Console.WriteLine("Fel inloggning!");
                    return null;
                }

                Session.LoggedInCustomer = customer;
                Console.WriteLine("Inloggning lyckades!");
                return customer;

            }
        }
        public static Models.Customer HandleLoginOrRegister() // fixa metoden för login och denna
            // om jag först loggar in i menyn och sen lägger produkter på varukorgen sen vill
            //checka ut, den kommer inte ihåg att har redan loggat in /måste fixas
        {
            // Om kunden är redan loggad vi använder den
            if (Session.LoggedInCustomer != null)
            {
                return Session.LoggedInCustomer;
            }

            Console.WriteLine("Du måste logga in eller registrera dig:");
            Console.WriteLine("1. Logga in");
            Console.WriteLine("2. Registrera dig");

            string choice = Console.ReadLine();

            if (choice == "1")
            {
                return RegisterCustomer.CustomerLogin();
            }
            else if (choice == "2")
            {
                return RegisterCustomer.RegisterNewCustomer();
            }
            else
            {
                Console.WriteLine("Fel val.");
                return null;
            }
        }
        public static void LogOut()
        {
            Session.LoggedInCustomer = null;
        }

    }
}
    




    


