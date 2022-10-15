using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.BLL.Repositories.OrderDetailRepository;
using Project.BLL.Repositories.OrderRepository;
using Project.BLL.Repositories.ProductRepository;
using Project.Common;
using Project.Entity.Entity;
using Project.Entity.Enum;
using Project.WEB.Models;
using Project.WEB.Utils;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Project.WEB.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderRepository orderRepository;
        private readonly IOrderDetailRepository orderDetailRepository;
        private readonly UserManager<AppUser> userManager;
        private readonly IProductRepository productRepository;

        public OrderController(IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository,UserManager<AppUser> userManager,IProductRepository productRepository)
        {
            this.orderRepository = orderRepository;
            this.orderDetailRepository = orderDetailRepository;
            this.userManager = userManager;
            this.productRepository = productRepository;
        }

        public IActionResult Index()
        {
            Cart cart = SessionHelper.GetProductFromJson<Cart>(HttpContext.Session, "sepet");
            return View(cart.Mycart);
        }

        public async Task<IActionResult> CompleteCart()
        {
            Cart cart = SessionHelper.GetProductFromJson<Cart>(HttpContext.Session, "sepet");

            if (User.Identity.IsAuthenticated)
            {
                CompleteCart completeCart = new();                
                int randomNumber = completeCart.RandomNumber();
                var user = await userManager.GetUserAsync(User);
                
                while (true)
                {
                    if (completeCart.RandomNumberCheck(orderRepository.GetAll(),randomNumber))
                    {
                        break;
                    }
                    randomNumber = completeCart.RandomNumber();
                }

                Order order = completeCart.AddOrder(user,cart,randomNumber);

                //Geçici yapıldı. Sayfa düzenlenecek
                order.PaymentMethod = PaymentMethod.Bankcard;
                orderRepository.Insert(order);

                foreach (CartItem cartItem in cart.Mycart)
                {
                    OrderDetail orderDetail = new();
                    orderDetail.ProductId = cartItem.Id;
                    orderDetail.UnitPrice = cartItem.UnitPrice;
                    orderDetail.Quantity = cartItem.Quantity;
                    orderDetail.OrderId = orderRepository.GetAll().Max(x => x.Id);

                    //Stoktan ürün miktarının düşülmesi için oluşturulmuştur. Ayrıca HomeController-AddToCart Action'ı için sepete stoktan fazla ürün eklenememesi için algoritma geliştirilmiştir.
                    Product product = productRepository.GetById(orderDetail.ProductId);
                    product.UnitsInStock =Convert.ToInt16(product.UnitsInStock - orderDetail.Quantity);
                    productRepository.Update(product);

                    orderDetailRepository.Insert(orderDetail);
                };                

                MailSender.SendEmail(user.Email, "Siparişiniz Oluşturuldu", $"#{order.OrderNumber} numaralı siparişiniz oluşturuldu. Kargoya verdiğimizde sizi bilgilendireceğiz!");
                SessionHelper.RemoveSession(HttpContext.Session, "sepet");
                return View(order);
            }
            return RedirectToAction("Index");
        }


    }
}
