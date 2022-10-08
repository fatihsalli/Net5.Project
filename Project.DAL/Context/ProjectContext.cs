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

        //==> IOC Containerdan talep ediyoruz bu sebeple de contructorda  yukarıdaki methodu gönderiyoruz. Bu methot IOC Container tarafında doldurularak Scope olarak instance alınıyor. O yüzden aşağıdaki methoda gerek kalmıyor.

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
