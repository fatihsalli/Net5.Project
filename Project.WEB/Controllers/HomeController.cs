using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Project.BLL.Repositories.CategoryRepository;
using Project.BLL.Repositories.ProductRepository;
using Project.WEB.Models;
using Project.WEB.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Project.WEB.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductRepository _productRepository;

        public HomeController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
       
        public IActionResult Index()
        {
            TempData["sepet"] = SessionHelper.GetProductFromJson<Cart>(HttpContext.Session, "sepet");

            return View(_productRepository.GetAll());
        }

        public IActionResult AddToCart(int id)
        {

            Cart cartSession;

            if (SessionHelper.GetProductFromJson<Cart>(HttpContext.Session,"sepet")==null)
            {
                cartSession = new Cart();
            }
            else
            {
                cartSession = SessionHelper.GetProductFromJson<Cart>(HttpContext.Session, "sepet");
            }
            
            var product=_productRepository.GetById(id);

            CartItem cartItem = new CartItem()
            {
                Id=product.Id,
                ProductName=product.ProductName,
                UnitPrice=product.UnitPrice
            };

            cartSession.AddItem(cartItem);

            SessionHelper.SetProductJson(HttpContext.Session, "sepet", cartSession);

            return RedirectToAction("Index");
        }

        public IActionResult MyCart()
        {
            if (SessionHelper.GetProductFromJson<Cart>(HttpContext.Session, "sepet") != null)
            {
                var sepet = SessionHelper.GetProductFromJson<Cart>(HttpContext.Session, "sepet");
                return View(sepet.Mycart);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }


    
    }
}
