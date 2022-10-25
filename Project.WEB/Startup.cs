using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Project.BLL.Repositories;
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
        /*Proje ASP.Net Core Web App(Model-View-Controller) se�ildikten sonra 
         * Authentication type=> Individual Accounts olarak se�ildi. 
         Bu sayede regin-logister-identity tan�mlamalar� otomatik geldi.*/

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //Context dahil etmek i�in
            services.AddDbContext<ProjectContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            //Bu, UseDeveloperExceptionPage ile birlikte Entity Framework ge�i�leri kullan�larak ��z�mlenebilen veritaban�yla ilgili �zel durumlar� yakalar. Bu �zel durumlar olu�tu�unda, sorunu ��zmek i�in olas� eylemler hakk�nda ayr�nt�l� bilgi i�eren bir HTML yan�t� olu�turulur.
            services.AddDatabaseDeveloperPageExceptionFilter();

            //Identity (kimlik y�netimi) dahil etme
            services.AddIdentity<AppUser,AppUserRole>(options => options.SignIn.RequireConfirmedEmail = false).AddEntityFrameworkStores<ProjectContext>().AddDefaultTokenProviders();

            //IdentityUser nesnesi i�erisinde bulunan varsay�lan �ifre tan�mlamalar�n� de�i�tirdik.
            services.Configure<IdentityOptions>(x =>
            {
                x.Password.RequireDigit = false;
                x.Password.RequiredLength = 6;
                x.Password.RequireNonAlphanumeric = false;
                x.Password.RequireUppercase = false;
                x.Password.RequireLowercase = false;

            });

            //MVC Dahil etmek i�in
            services.AddControllersWithViews();

            //Instancelar
            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
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
                x.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/Home/Login");
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
            //E�er uygulama geli�tirme a�amas�nda a�a��daki kod tan�ml� ise al�nan hatalar browserda g�sterilir=>UseDeveloperExceptionPage
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

            //wwwroot
            app.UseStaticFiles();

            //UseRouting url'de istekleri tan�mlamak i�in kullan�lan metot.
            app.UseRouting();

            //Session'� aktif hale getirmek i�in
            app.UseSession();

            //Authentication => Kimlik y�netimi 
            app.UseAuthentication();

            //Authorization=> Yetkilendirme 
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
