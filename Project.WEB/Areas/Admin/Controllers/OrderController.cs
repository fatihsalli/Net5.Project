using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.BLL.Repositories.OrderDetailRepository;
using Project.BLL.Repositories.OrderRepository;

namespace Project.WEB.Areas.Admin.Controllers
{
    //[Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IOrderRepository orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        public IActionResult Index()
        {
            decimal income = 0;
            foreach (var item in orderRepository.GetAll())
            {
                income += item.TotalPrice;
            }
            TempData["TotalIncome"] = income;


            TempData["Title"] = "Orders";
            return View(orderRepository.GetAll());
        }

    }
}
