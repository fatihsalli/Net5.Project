using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.Entity.Entity;

namespace Project.WEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> userManager;

        public UserController(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            return View(userManager.Users);
        }
    }
}
