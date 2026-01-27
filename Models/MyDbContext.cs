using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VardagshörnanApp.Models
{
    internal class MyDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Administrator>Administrators { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .Build();

            var connStr = config["MySettings:ConnectionString"];
            optionsBuilder.UseSqlServer(connStr);
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(@"Server=tcp:wafaesdb.database.windows.net,1433;Initial Catalog=MyDbWafae;Persist Security Info=False;User ID=dbadmin;Password=System25Demo;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        //   //optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS01;Database=VardagshörnanDb;Trusted_Connection=True;TrustServerCertificate=True;");

        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder) // för att inte ta bort en kategori som innehåller produkter
        {
            modelBuilder.Entity<Product>()
           .HasOne(p => p.Category)
           .WithMany(c => c.Products)
           .HasForeignKey(p => p.CategoryId)
           .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
