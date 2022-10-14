using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.BLL.Repositories.OrderDetailRepository;
using Project.BLL.Repositories.OrderRepository;
using Project.BLL.Repositories.ShipperRepository;
using Project.Common;
using Project.Entity.Entity;
using Project.WEB.Areas.Admin.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Project.WEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ShipperController : Controller
    {
        private readonly IOrderRepository orderRepository;
        private readonly IShipperRepository shipperRepository;
        private readonly UserManager<AppUser> userManager;

        public ShipperController(IOrderRepository orderRepository,IShipperRepository shipperRepository, UserManager<AppUser> userManager)
        {
            this.orderRepository = orderRepository;
            this.shipperRepository = shipperRepository;
            this.userManager = userManager;
        }

        public IActionResult Index(int orderId)
        {
            TransformHelper.transformObject = orderRepository.GetById(orderId);
            return View(shipperRepository.GetAll());
        }

        public async Task<IActionResult> Select(int shipperId)
        {
            Order order = (Order)TransformHelper.transformObject;
            order.ShipperId = shipperId;
            order.IsShipped = true;
            orderRepository.Update(order);

            var user = await userManager.FindByIdAsync(order.UserId.ToString());

            MailSender.SendEmail(user.Email, "Siparişiniz Kargoya Verilmiştir", $"#{order.OrderNumber} numaralı siparişiniz kargoya verilmiştir!");

            return RedirectToAction("Index","Home");
        }

        public IActionResult Detail(int id)
        {
            Order order=orderRepository.GetById(id);
            ViewBag.OrderNumber = order.OrderNumber;
            Shipper shipper=shipperRepository.GetById(Convert.ToInt32(order.ShipperId));
            return View(shipper);
        }

        public IActionResult Create()
        {
            Order order = (Order)TransformHelper.transformObject;
            ViewData["Order Number"] = order.OrderNumber;
            return View();
        }

        [HttpPost]
        public IActionResult Create(Shipper fakeShipper)
        {
            Shipper shipper= new Shipper();
            shipper.CompanyName = fakeShipper.CompanyName;
            shipper.Address = fakeShipper.Address;
            shipperRepository.Insert(shipper);
            Order order = (Order)TransformHelper.transformObject;
            //Action / Controller / routeValue ile orderid'yi başka actiona gönderebildik.
            return RedirectToAction("Index","Shipper",new { @orderId = order.Id });
        }


    }
}
