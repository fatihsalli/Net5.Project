using Microsoft.AspNetCore.Mvc;
using Project.BLL.Repositories.OrderDetailRepository;
using Project.BLL.Repositories.OrderRepository;

namespace Project.WEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public IActionResult Index()
        {
            @TempData["Title"] = "Orders";
            return View(_orderRepository.GetAll());
        }

    }
}
