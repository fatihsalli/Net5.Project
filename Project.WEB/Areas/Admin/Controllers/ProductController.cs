using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Project.BLL.Repositories.CategoryRepository;
using Project.BLL.Repositories.ProductRepository;
using Project.Entity.Entity;
using Project.WEB.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
        public async Task<IActionResult> Create(Product product,IFormFile imageFile)
        {
            try
            {
                string path;
                if (imageFile==null)
                {
                    //Varsayılan görsel
                    path = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot\\images\\", "no-image.jpg");
                    product.ImagePath = "no-image.jpg";
                    TempData["Result"] = "Ürün varsayılan görsel ile eklendi!";
                }
                else
                {
                    var fileArray = imageFile.FileName.Split(".");
                    var extension = fileArray[fileArray.Length - 1].ToLower();

                    if (extension=="jpg" || extension=="jpeg" || extension=="png")
                    {
                        var uniqueName = Guid.NewGuid();
                        var newFileName = uniqueName + "." + extension;
                        //Kullanıcıdan gelen görsel
                        path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\", newFileName);
                        //Alternatif kütüphane ekleme yöntemi. Bu controllerda sadece burada kullanacaksak bu şekilde ekleyebiliriz. Filestream sayesinde kullanıcının eklediği dosyayı images klasörümüz içerisine kayıt edebiliyoruz.
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(stream);
                        }
                        TempData["Result"] = "Ürün seçilen görsel ile eklendi!";
                        product.ImagePath = newFileName;
                    }
                    else
                    {
                        path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\", "no-image.jpg");
                        product.ImagePath = "no-image.jpg";
                        TempData["Result"] = "Seçilen format uygun olmadığı için ürün varsayılan görsel ile eklendi!";
                    }                                  
                }
            }
            catch (System.Exception ex)
            {
                TempData["Result"]=ex.Message;
            }
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

            //Update ederken imagepathi null'a çekip update etmiyor sorulacak?
            TransformHelper.transformObject = product.ImagePath;
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
            product.ImagePath =(string)TransformHelper.transformObject;
            productRepository.Update(product);
            return RedirectToAction("Index");
        }
    }
}
