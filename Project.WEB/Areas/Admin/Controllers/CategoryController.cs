using Microsoft.AspNetCore.Mvc;
using Project.BLL.Repositories.CategoryRepository;
using Project.BLL.Repositories.ProductRepository;
using Project.Entity.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Project.WEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;

        public CategoryController(ICategoryRepository categoryRepository,IProductRepository productRepository)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
        }

        public IActionResult Index()
        {
            @TempData["Title"] = "Category";
            //ViewBag.Categories = _productRepository.GetAll();
            return View(_categoryRepository.GetAll());
        }

        [HttpGet]
        public IActionResult Create()
        {
            @TempData["Title"] = "Create Category";
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            _categoryRepository.Insert(category);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var category = _categoryRepository.GetById(id);
            _categoryRepository.Remove(category);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            @TempData["Title"] = "Update Category";
            var category= _categoryRepository.GetById(id);
            return View(category);
        }

        [HttpPost]
        public IActionResult Update(Category category)
        {
            _categoryRepository.Update(category);
            return RedirectToAction("Index");
        }



    }
}
