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
            //Order Details içinde ProductName'i de gösterebilmek için eklenmiştir.
            List<Product> listProduct = productRepository.GetAll().ToList();
            ViewBag.ProductList = listProduct;
            TempData["Title"] = "Order Details";
            return View(orderDetailRepository.GetAll());
        }


    }
}
