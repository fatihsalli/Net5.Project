using Microsoft.AspNetCore.Mvc;

namespace Project.WEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            @TempData["Title"] = "Category";
            return View();
        }
    }
}
