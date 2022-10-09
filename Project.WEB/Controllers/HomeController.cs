using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Project.BLL.Repositories.CategoryRepository;
using Project.BLL.Repositories.ProductRepository;
using Project.WEB.Models;
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
            return View(_productRepository.GetAll());
        }
    
    }
}
