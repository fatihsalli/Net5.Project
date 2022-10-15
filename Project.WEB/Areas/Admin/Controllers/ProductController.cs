using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Project.BLL.Repositories.CategoryRepository;
using Project.BLL.Repositories.ProductRepository;
using Project.Entity.Entity;
using Project.WEB.Areas.Admin.Models;
using System.Collections.Generic;
using System.Linq;

namespace Project.WEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IProductRepository productRepository;
        private readonly ICategoryRepository categoryRepository;
        public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            this.productRepository = productRepository;
            this.categoryRepository = categoryRepository;   
        }

        public IActionResult Index()
        {
            //Product List içerisinde Category Name de gösterilebilmesi için eklenmiştir.
            List<Category> listCategory = categoryRepository.GetAll().ToList();
            ViewBag.CategoryList = listCategory;
            TempData["Title"] = "Product";
            return View(productRepository.GetAll());
        }

        [HttpGet]
        public IActionResult Create()
        {
            TempData["Title"] = "Create Product";

            ViewBag.Categories = categoryRepository.GetAll().Select(x=> new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem() 
            { 
                Text= x.CategoryName, Value=x.Id.ToString()
            });

            return View();
        }

        [HttpPost]
        public IActionResult Create(Product product)
        {
            productRepository.Insert(product);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var product = productRepository.GetById(id);
            productRepository.Remove(product);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            TempData["Title"] = "Update Product";
            var product = productRepository.GetById(id);
            ViewBag.Categories = categoryRepository.GetAll().Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem()
            {
                Text = x.CategoryName,
                Value = x.Id.ToString()
            });
            return View(product);
        }

        [HttpPost]
        public IActionResult Update(Product product)
        {
            productRepository.Update(product);
            return RedirectToAction("Index");
        }
    }
}
