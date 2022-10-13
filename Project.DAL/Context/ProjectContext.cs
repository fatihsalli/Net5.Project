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
    //AppUser ve AppUserRole isimlerinde class oluşturarak IdentityUserdan miras aldırıyoruz. Sonrasında Guid olan tipi int yaptık ve order tarafında Usersları bağlamış olduk. Id'nin int yapılmış olması doğru bir yaklaşım değil ancak burada "AppUser ve AppUserRole" üzerinden IdentityUser'daki Id tipini değiştirmiş olduk.
    public class ProjectContext:IdentityDbContext<AppUser,AppUserRole,int>
    {
        //==> IOC Containerdan talep ediyoruz bu sebeple de contructorda  yukarıdaki methodu gönderiyoruz. Bu methot IOC Container tarafında doldurularak Scope olarak instance alınıyor. O yüzden aşağıdaki methoda gerek kalmıyor.
        public ProjectContext(DbContextOptions<ProjectContext> options) : base(options)
        {

        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Shipper> Shippers { get; set; }


        

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

            //Shipper Düzenlemeler
            modelBuilder.Entity<Shipper>().Property(x => x.CompanyName).IsRequired(true);
            modelBuilder.Entity<Shipper>().Property(x => x.CompanyName).HasMaxLength(100);
            modelBuilder.Entity<Shipper>().Property(x => x.Address).HasMaxLength(250);

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
