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
using Project.BLL.Repositories.ShipperRepository;
using Project.DAL.Context;
using Project.Entity.Entity;
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
            services.AddIdentity<AppUser,AppUserRole>(options => options.SignIn.RequireConfirmedEmail = false).AddEntityFrameworkStores<ProjectContext>().AddDefaultTokenProviders();

            //MVC Dahil etmek için
            services.AddControllersWithViews();

            //Instancelar
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
            services.AddScoped<IShipperRepository, ShipperRepository>();

            //Sepet
            services.AddSession(x =>
            {
                x.Cookie.Name = "project_Cart";
                x.IOTimeout=TimeSpan.FromMinutes(1);
            });

            //Cookie
            services.ConfigureApplicationCookie(x =>
            {
                x.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Home/Login");
                x.Cookie = new Microsoft.AspNetCore.Http.CookieBuilder
                {
                    Name = "Login_Cookie"
                };
                x.SlidingExpiration = true;
                x.ExpireTimeSpan = TimeSpan.FromMinutes(1);
            });
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

            //Session'ý aktif hale getirmek için
            app.UseSession();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //Area Route
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllerRoute(
                      name: "areas",
                      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                    );
                });

                //Confirmation Route
                endpoints.MapControllerRoute(
                    name: "confirmation",
                    pattern: "{controller=Home}/{action=Confirmation}/{id}/{code}");

                //Default Route
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
