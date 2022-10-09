using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.BLL.Repositories.CategoryRepository;
using Project.BLL.Repositories.ProductRepository;
using Project.Entity.Entity;

namespace Project.WEB.Areas.Admin.Controllers
{
    //[Authorize(Roles ="Admin")]
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public IActionResult Index()
        {
            @TempData["Title"] = "Product";
            return View(_productRepository.GetAll());
        }

        [HttpGet]
        public IActionResult Create()
        {
            @TempData["Title"] = "Create Product";
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
