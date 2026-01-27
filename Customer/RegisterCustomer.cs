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
            if (Session.LoggedInCustomer != null)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                List<string> topText1 = new List<string> { "", "      Du är redan inloggad!       ", "" };
                var window = new Window("", 50, 15, topText1);
                window.Draw();
                Thread.Sleep(1000);
                Console.ResetColor();
                Thread.Sleep(1000);
                return Session.LoggedInCustomer; // returnera den kunden som är redan loggad
            }
            Console.Clear();
            Common.WelcomeTextWindow();
            Console.SetCursorPosition(0, 12);
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
                        Console.ForegroundColor= ConsoleColor.Red;
                        Console.WriteLine("Denna e-post är redan registrerad, försök en annan.");
                        Console.ResetColor();
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
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Lösenorden matchar inte. Försök igen.");
                        Console.ResetColor();
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
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Registrering misslyckades: " + ex.Message);
                    Console.ResetColor();
                }
                
                Session.LoggedInCustomer = customer;
                CustomerPage.LoggedInCustomer= customer;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Grattis, " + customer.FirstName + "! Din registrering är klar.");
                Console.WriteLine();
                Console.ResetColor();
                Console.WriteLine("Användarnamn: " + customer.EmailAdress);
                Console.ReadKey();
                return customer;
            }
        }
        public static Models.Customer CustomerLogin()
        {
            if (Session.LoggedInCustomer != null)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                List<string> topText1 = new List<string> { "", "      Du är redan inloggad!       ", "" };
                var window = new Window("", 50, 15, topText1);
                window.Draw();
                Thread.Sleep(1000);
                Console.ResetColor();
                return Session.LoggedInCustomer; // returnera den kunden som är redan loggad
            }
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
                    Console.ForegroundColor = ConsoleColor.Red;
                    List<string> topText1 = new List<string> { "", "      Fel loggning!       ", "" };
                    var window = new Window("", 50, 15, topText1);
                    window.Draw();
                    Thread.Sleep(1000);
                    return null;
                }

                CustomerPage.LoggedInCustomer = customer; 
                Session.LoggedInCustomer = customer;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Clear();
                List<string> topText = new List<string> { "", "      Inloggning lyckades!       ", "" };
                var waitingWindow = new Window("", 50, 15, topText);
                waitingWindow.Draw();
                Console.ResetColor();
                Thread.Sleep(1000);
                return customer;

            }
        }
        public static Models.Customer HandleLoginOrRegister() 
        {
            if (Session.LoggedInCustomer != null)
            {
                return Session.LoggedInCustomer;
            }

            List<string> topText1 = new List<string> { "1. Logga in","2. Registrera dig" };
            var window = new Window("Du behöver logga in eller registrera dig: ", 50, 15, topText1);
            window.Draw();
            string choice = Console.ReadLine();

            if (choice == "1")
            {
                return CustomerLogin();
            }
            else if (choice == "2")
            {
                return RegisterNewCustomer();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Fel val.");
                Console.ResetColor();
                return null;
            }
        }
        public static void LogOut()
        {
            Session.LoggedInCustomer = null;
        }

    }
}
    




    


