using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json.Linq;
using Project.BLL.Repositories.CategoryRepository;
using Project.BLL.Repositories.ProductRepository;
using Project.Entity.Entity;
using Project.WEB.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace Project.WEB.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IProductRepository productRepository;
        public CategoryController(ICategoryRepository categoryRepository,IProductRepository productRepository)
        {
            this.categoryRepository = categoryRepository;
            this.productRepository = productRepository;
        }

        public IActionResult Index()
        {
            //TotalProduct'ı bulabilmek için oluşturulmuştur.
            List<Product> productList=productRepository.GetAll().ToList();
            ViewBag.ProductList=productList;
            TempData["Title"] = "Category";
            return View(categoryRepository.GetAll());
        }

        [HttpGet]
        public IActionResult Create()
        {
            TempData["Title"] = "Create Category";
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            categoryRepository.Insert(category);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var category = categoryRepository.GetById(id);
            categoryRepository.Remove(category);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            TempData["Title"] = "Update Category";
            var category= categoryRepository.GetById(id);
            return View(category);
        }

        [HttpPost]
        public IActionResult Update(Category category)
        {
            categoryRepository.Update(category);
            return RedirectToAction("Index");
        }



    }
}
