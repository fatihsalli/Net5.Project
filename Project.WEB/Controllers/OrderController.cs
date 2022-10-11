using Microsoft.AspNetCore.Mvc;
using Project.BLL.Repositories.OrderDetailRepository;
using Project.BLL.Repositories.OrderRepository;
using Project.Entity.Entity;
using Project.WEB.Models;
using Project.WEB.Utils;
using System.Linq;

namespace Project.WEB.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;

        public OrderController(IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository)
        {
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
        }

        public IActionResult Index()
        {
            Cart cart = SessionHelper.GetProductFromJson<Cart>(HttpContext.Session, "sepet");
            return View(cart.Mycart);
        }

        public IActionResult AddOrder()
        {
            Cart cart = SessionHelper.GetProductFromJson<Cart>(HttpContext.Session, "sepet");
            Order order = new();
            decimal totalPrice = 0;

            foreach (CartItem cartItem in cart.Mycart)
            {
                totalPrice += cartItem.SubTotal;
            }

            order.TotalPrice = totalPrice;
            _orderRepository.Insert(order);

            foreach (CartItem cartItem in cart.Mycart)
            {
                OrderDetail orderDetail = new();
                orderDetail.ProductId = cartItem.Id;
                orderDetail.UnitPrice = cartItem.UnitPrice;
                orderDetail.Count = cartItem.Quantity;
                orderDetail.OrderId = _orderRepository.GetAll().Max(x=> x.Id);
                _orderDetailRepository.Insert(orderDetail);
            };

            return View(_orderRepository.GetAll().Max(x => x.Id));
        }


    }
}
