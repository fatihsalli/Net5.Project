using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.Entity.Entity;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Project.WEB.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
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

        //Assign Roles
        public async Task<IActionResult> AssignAdminRole(string id)
        {
            var user= await userManager.FindByIdAsync(id);

            if (user!=null)
            {
                IEnumerable<string> roles = new List<string>
                {
                    "Admin","Accountant"
                };

                var result= await userManager.AddToRolesAsync(user, roles);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> AssignAccountantRole(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user != null)
            {
                string role = "Accountant";

                var result = await userManager.AddToRoleAsync(user, role);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }











    }
}
