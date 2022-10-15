using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.BLL.Repositories.CategoryRepository;
using Project.BLL.Repositories.OrderDetailRepository;
using Project.BLL.Repositories.ProductRepository;
using Project.Entity.Entity;
using Project.WEB.Areas.Admin.Models;
using System.Collections.Generic;
using System.Linq;

namespace Project.WEB.Areas.Admin.Controllers
{
    //[Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class OrderDetailController : Controller
    {
        private readonly IOrderDetailRepository orderDetailRepository;
        private readonly IProductRepository productRepository;

        public OrderDetailController(IOrderDetailRepository orderDetailRepository,IProductRepository productRepository)
        {
            this.orderDetailRepository = orderDetailRepository;
            this.productRepository = productRepository;
        }

        public IActionResult Index()
        {
            TempData["Title"] = "Order Details";
            List<Product> listProduct = productRepository.GetAll().ToList();
            ViewBag.ProductList = listProduct;
            return View(orderDetailRepository.GetAll());
        }


    }
}
