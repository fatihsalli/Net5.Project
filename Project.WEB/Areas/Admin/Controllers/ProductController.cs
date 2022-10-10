using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Project.BLL.Repositories.CategoryRepository;
using Project.BLL.Repositories.ProductRepository;
using Project.Entity.Entity;
using System.Linq;

namespace Project.WEB.Areas.Admin.Controllers
{
    //[Authorize(Roles ="Admin")]
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;


        public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;   
        }

        public IActionResult Index()
        {
            TempData["Title"] = "Product";
            return View(_productRepository.GetAll());
        }

        [HttpGet]
        public IActionResult Create()
        {
            TempData["Title"] = "Create Product";
            //ViewBag.Categories=_categoryRepository.GetAll();
            ViewBag.Categories = _categoryRepository.GetAll().Select(x=> new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem() 
            { 
                Text= x.CategoryName, Value=x.Id.ToString()
            });

            return View();
        }

        [HttpPost]
        public IActionResult Create(Product product)
        {
            _productRepository.Insert(product);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var product = _productRepository.GetById(id);
            _productRepository.Remove(product);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            @TempData["Title"] = "Update Product";
            var product = _productRepository.GetById(id);
            ViewBag.Categories = _categoryRepository.GetAll().Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem()
            {
                Text = x.CategoryName,
                Value = x.Id.ToString()
            });
            return View(product);
        }

        [HttpPost]
        public IActionResult Update(Product product)
        {
            _productRepository.Update(product);
            return RedirectToAction("Index");
        }
    }
}
