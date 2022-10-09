using Microsoft.AspNetCore.Mvc;
using Project.BLL.Repositories.OrderDetailRepository;
using Project.Entity.Entity;

namespace Project.WEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderDetailController : Controller
    {
        private readonly IOrderDetailRepository _orderDetailRepository;

        public OrderDetailController(IOrderDetailRepository orderDetailRepository)
        {
            _orderDetailRepository = orderDetailRepository;
        }

        public IActionResult Index()
        {
            @TempData["Title"] = "Order Details";
            return View(_orderDetailRepository.GetAll());
        }


    }
}
