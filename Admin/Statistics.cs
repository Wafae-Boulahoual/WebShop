using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VardagshörnanApp.Admin
{
    internal class Statistics
    {
        public static void StatisticsWindow()
        {
            List<string> topText = new List<string> { "", "W. Statistik", "Q. Start sida", "" };
            Console.ForegroundColor = ConsoleColor.Green;
            var windowTop = new Window("Välj en alternativ", 112, 15, topText);
            Console.ResetColor();
            windowTop.Draw();
        }
        private static SqlConnection OpenConnection() // connection sträng på user-secrets
        {
            var builder = new ConfigurationBuilder()
                .AddUserSecrets<Statistics>();
            var configuration = builder.Build();
            string connString = configuration["MySettings:ConnectionString"];
            return new SqlConnection(connString);
        }
        public static void AllQueries()
        {
            Console.Clear();
            Top3ProductsWindow();
            TopCategoryWindow();
            Top3SuppliersWindow();
            OrdersPerCityWindow();
            Top3CitiesWindow();
            Console.ReadKey();


        }
        public static void Top3ProductsWindow()
        {
            using var connection = OpenConnection();
            string sql = @" SELECT TOP 3 p.Name, SUM(oi.Quantity) AS TotalSold
                            FROM OrderItems oi
                            JOIN Products p ON oi.ProductId = p.Id
                            GROUP BY p.Name
                            ORDER BY TotalSold DESC";

            var products = connection.Query(sql);
            List<string> productText = new List<string>{""};
            foreach (var p in products)
            {
                productText.Add( p.TotalSold + " st " + p.Name);
            }
            var window = new Window("Top 3 produkter:", 2, 2, productText);
            window.Draw();
        }
        public static void TopCategoryWindow()
        {
            using var connection = OpenConnection();

            string sql = @"SELECT TOP 1 c.Name, SUM(oi.Quantity) AS TotalSold
                            FROM OrderItems oi
                            JOIN Products p ON oi.ProductId = p.Id
                            JOIN Categories c ON p.CategoryId = c.Id
                            GROUP BY c.Name
                            ORDER BY TotalSold DESC";

            var cat = connection.QuerySingle(sql);
            List<string> text = new List<string>{cat.TotalSold + " st " + cat.Name};
            var window = new Window("Populäraste kategori", 2, 11, text);
            window.Draw();
        }
        public static void OrdersPerCityWindow()
        {
            using var connection = OpenConnection();

            string sql = @" SELECT TOP 3 c.City, COUNT(o.Id) AS OrdersCount
                            FROM Orders o
                            JOIN Customers c ON o.CustomerId = c.Id
                            GROUP BY c.City
                            ORDER BY OrdersCount DESC";
            var cities = connection.Query(sql);
            List<string> text = new List<string>{""};
            foreach (var c in cities)
            {
                text.Add(c.OrdersCount + " st " + c.City);
            }
            var window = new Window("Flest beställningar per stad",2 , 15, text);
            window.Draw();
        }
        public static void Top3SuppliersWindow()
        {
            using var connection = OpenConnection();

            string sql = @"SELECT TOP 3 p.Supplier, SUM(oi.Quantity * oi.Price) AS TotalSales
                           FROM OrderItems oi
                           JOIN Products p ON oi.ProductId = p.Id
                           GROUP BY p.Supplier
                           ORDER BY TotalSales DESC;";

            var suppliers = connection.Query(sql);
            List<string> text = new List<string> { "" };

            foreach (var s in suppliers)
            {
                text.Add(s.Supplier + " - " + s.TotalSales + " kr");
            }

            var window = new Window("Top 3 leverantörer", 2, 22, text);
            window.Draw();
        }
        public static void Top3CitiesWindow()
        {
            using var connection = OpenConnection();
            string sql = @"SELECT TOP 5 c.City, COUNT(o.Id) AS OrdersCount
                            FROM Orders o
                            JOIN Customers c ON o.CustomerId = c.Id
                            GROUP BY c.City
                            ORDER BY OrdersCount DESC";

            var cities = connection.Query(sql);
            List<string> text = new List<string> { "" };
            foreach (var c in cities)
            {
                text.Add(c.OrdersCount + " st " + c.City);
            }
            var window = new Window("Flest beställningar per stad", 2, 30, text);
            window.Draw();
        }
    }
}

