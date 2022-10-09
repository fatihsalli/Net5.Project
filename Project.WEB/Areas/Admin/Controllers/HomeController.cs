using Microsoft.AspNetCore.Mvc;
using Project.BLL.Repositories.OrderRepository;

namespace Project.WEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            @TempData["Title"] = "HomePage";
            return View();
        }
    }
}
