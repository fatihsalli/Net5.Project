using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Project.Entity.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DAL.Context
{
    public class ProjectContext:IdentityDbContext
    {
        public ProjectContext(DbContextOptions<ProjectContext> options) : base(options)
        {

        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }


        //==> IOC Containerdan talep ediyoruz bu sebeple de contructorda  yukarıdaki methodu gönderiyoruz. Bu methot IOC Container tarafında doldurularak Scope olarak instance alınıyor. O yüzden aşağıdaki methoda gerek kalmıyor.

        //Database Tasarımı
        //Fluent Api yöntemiyle database ayarlarını güncelliyoruz.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //OrderDetail ara tablosu için
            modelBuilder.Entity<OrderDetail>().Ignore(x => x.Id);
            modelBuilder.Entity<OrderDetail>().HasKey(x=> new {x.OrderId, x.ProductId });

            //Product düzenlemeler
            modelBuilder.Entity<Product>().Property(x => x.ProductName).IsRequired(true);
            modelBuilder.Entity<Product>().Property(x => x.ProductName).HasMaxLength(50);
            modelBuilder.Entity<Product>().Property(x => x.Description).HasMaxLength(250);
            modelBuilder.Entity<Product>().Property(x => x.ImagePath).HasMaxLength(500);

            //Category Düzenlemeler
            modelBuilder.Entity<Category>().Property(x => x.CategoryName).IsRequired(true);
            modelBuilder.Entity<Category>().Property(x => x.CategoryName).HasMaxLength(50);
            modelBuilder.Entity<Category>().Property(x => x.Description).HasMaxLength(250);

            base.OnModelCreating(modelBuilder);
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseSqlServer("server=FATIH\\SQLEXPRESS;database=Net5Project;uid=sa;pwd=123;");
        //    }
        //    base.OnConfiguring(optionsBuilder);
        //}
    }
}
