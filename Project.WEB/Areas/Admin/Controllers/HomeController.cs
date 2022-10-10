using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.BLL.Repositories.OrderRepository;
using Project.BLL.Repositories.ProductRepository;
using System.Linq;

namespace Project.WEB.Areas.Admin.Controllers
{
    //[Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly IProductRepository productRepository;
        private readonly IOrderRepository orderRepository;
        private readonly UserManager<IdentityUser> userManager;


        public HomeController(IProductRepository productRepository,IOrderRepository orderRepository,UserManager<IdentityUser> userManager)
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

            return View(income);
        }
    }
}
