using Dapper;
using Microsoft.Data.SqlClient;
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
            List<string> topText = new List<string> { "","W. Statistik", "Q. Start sida","" };
            Console.ForegroundColor = ConsoleColor.Green;
            var windowTop = new Window("Välj en alternativ", 112, 15, topText);
            Console.ResetColor();
            windowTop.Draw();
        }

        static string connString = "Server=tcp:wafaesdb.database.windows.net,1433;Initial Catalog=MyDbWafae;Persist Security Info=False;User ID=dbadmin;Password=System25Demo;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public static List<Models.Order> GetAllOrders()
        {
            string sql = "SELECT * FROM Orders";
            List<Models.Order> orders = new List<Models.Order>();

            using (var connection = new SqlConnection(connString))
            {
                orders = connection.Query<Models.Order>(sql).ToList();
            }
            return orders;
        }
        public static void Queries()
        {
           
        }
    }
}
