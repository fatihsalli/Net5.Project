using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Project.BLL.Repositories.CategoryRepository;
using Project.BLL.Repositories.OrderDetailRepository;
using Project.BLL.Repositories.OrderRepository;
using Project.BLL.Repositories.ProductRepository;
using Project.DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.WEB
{
    public class Startup
    {
        /*Proje ASP.Net Core Web App(Model-View-Controller) seçildikten sonra 
         * Authentication type=> Individual Accounts olarak seçildi. 
         Bu sayede regin-logister-identity tanýmlamalarý otomatik geldi.*/

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            //Context dahil etmek için
            services.AddDbContext<ProjectContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            //Bu, UseDeveloperExceptionPage ile birlikte Entity Framework geçiþleri kullanýlarak çözümlenebilen veritabanýyla ilgili özel durumlarý yakalar. Bu özel durumlar oluþtuðunda, sorunu çözmek için olasý eylemler hakkýnda ayrýntýlý bilgi içeren bir HTML yanýtý oluþturulur.
            services.AddDatabaseDeveloperPageExceptionFilter();

            //Kimlik Yönetimi
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ProjectContext>();

            //MVC DAhil etmek için
            services.AddControllersWithViews();

            //Instancelar
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();


        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllerRoute(
                      name: "areas",
                      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                    );
                });
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
