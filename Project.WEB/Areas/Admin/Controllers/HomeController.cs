using Microsoft.AspNetCore.Mvc;
using Project.BLL.Repositories.OrderRepository;
using Project.BLL.Repositories.ProductRepository;
using System.Linq;

namespace Project.WEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly IProductRepository productRepository;
        private readonly IOrderRepository orderRepository;

        public HomeController(IProductRepository productRepository,IOrderRepository orderRepository)
        {
            this.productRepository = productRepository;
            this.orderRepository = orderRepository;
        }

        public IActionResult Index()
        {
            @TempData["Title"] = "HomePage";
            @TempData["Orders"] =orderRepository.GetAll().Count();
            @TempData["Products"] = productRepository.GetAll().Count();
            decimal income = 0;
            foreach (var item in orderRepository.GetAll())
            {
                income += item.TotalPrice;
            }

            return View(income);
        }
    }
}
