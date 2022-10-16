using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.BLL.Repositories.OrderRepository;
using Project.BLL.Repositories.ProductRepository;
using Project.Entity.Entity;
using System.Linq;

namespace Project.WEB.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,Accountant")]
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly IProductRepository productRepository;
        private readonly IOrderRepository orderRepository;
        private readonly UserManager<AppUser> userManager;
        public HomeController(IProductRepository productRepository,IOrderRepository orderRepository,UserManager<AppUser> userManager)
        {
            this.productRepository = productRepository;
            this.orderRepository = orderRepository;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            TempData["Title"] = "HomePage";
            TempData["Orders"] =orderRepository.GetAll().Count();
            TempData["Products"] = productRepository.GetAll().Count();
            TempData["Users"] = userManager.Users.Count();

            decimal income = 0;
            foreach (var item in orderRepository.GetAll())
            {
                income += item.TotalPrice;
            }
            TempData["TotalIncome"] = income;

            return View(orderRepository.GetAll().OrderByDescending(x=> x.Id));
        }

    }
}
