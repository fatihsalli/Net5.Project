using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Project.BLL.Repositories.CategoryRepository;
using Project.BLL.Repositories.ProductRepository;
using Project.Common;
using Project.Entity.Entity;
using Project.WEB.Models;
using Project.WEB.Models.ViewModels;
using Project.WEB.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Project.WEB.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductRepository productRepository;
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;

        public HomeController(IProductRepository productRepository, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            this.productRepository = productRepository;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
       
        public IActionResult Index()
        {
            TempData["sepet"] = SessionHelper.GetProductFromJson<Cart>(HttpContext.Session, "sepet");

            return View(productRepository.GetAll());
        }

        public IActionResult AddToCart(int id)
        {

            Cart cartSession;

            if (SessionHelper.GetProductFromJson<Cart>(HttpContext.Session,"sepet")==null)
            {
                cartSession = new Cart();
            }
            else
            {
                cartSession = SessionHelper.GetProductFromJson<Cart>(HttpContext.Session, "sepet");
            }
            
            var product= productRepository.GetById(id);

            CartItem cartItem = new CartItem()
            {
                Id=product.Id,
                ProductName=product.ProductName,
                UnitPrice=product.UnitPrice
            };

            cartSession.AddItem(cartItem);

            SessionHelper.SetProductJson(HttpContext.Session, "sepet", cartSession);

            return RedirectToAction("Index");
        }

        public IActionResult MyCart()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (SessionHelper.GetProductFromJson<Cart>(HttpContext.Session, "sepet") != null)
                {
                    Cart sepet = SessionHelper.GetProductFromJson<Cart>(HttpContext.Session, "sepet");
                    return View(sepet.Mycart);
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerUser)
        {
            if (ModelState.IsValid)
            {
                AppUser newUser = new AppUser()
                {
                    UserName= registerUser.Username,
                    Email=registerUser.Email
                };

                var result = await userManager.CreateAsync(newUser, registerUser.Password);

                var registerToken = "";

                if (result.Succeeded)
                {
                    //Bu kısım araştırılacak sorun var!!!
                    while (true)
                    {
                        registerToken = userManager.GenerateEmailConfirmationTokenAsync(newUser).Result;
                        if (!registerToken.Contains("/"))
                        {
                            break;
                        }
                    }

                    MailSender.SendEmail(registerUser.Email, "Register", $"Merhaba {registerUser.Username}! kayıt işleminiz başarılı şekilde oluşturuldu! Üyeliği tamamlamak için linke tıklayın https://localhost:5001/home/confirmation/" + newUser.Id + "/" + registerToken);

                    //var callBackUrl = Url.Action("Confirmation", "Home", new { id = newUser.Id, registerCode = registerToken }, protocol: Request.Scheme);

                    //MailSender.SendEmail(registerUser.Email, "Register", $"Merhaba! {registerUser.Username} kayıt işleminiz başarılı şekilde oluşturuldu! Üyeliği tamamlamak için linke tıklayın: <a href=\""+callBackUrl+ "\">link</a>");

                    TempData["result"] = $"{newUser.Email} adresine aktivasyon maili gönderdik. Üyeliğinizi aktif hale getirmek için ilgili linki tıklayın.";

                    //return RedirectToAction("Confirmation", new { id = newUser.Id, registerCode = registerToken.Result});
                    return View(registerUser);
                }
                else
                {
                    return View(registerUser);
                }
            }
            return View(registerUser);
        }

        public async Task<IActionResult> Confirmation(string id, string registerCode)
        {
            registerCode = registerCode.ToString();
            if (id!=null && registerCode != null)
            {
                var user = await userManager.FindByIdAsync(id);
                var confirm= await userManager.ConfirmEmailAsync(user, registerCode);
                if (confirm.Succeeded)
                {
                    return RedirectToAction("Login");
                }
            }
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(loginVM.Username);
                if (user!=null)
                {
                    //
                    var result = await signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);
                    if (result.Succeeded)
                    {
                        if (SessionHelper.GetProductFromJson<Cart>(HttpContext.Session, "sepet") != null)
                        {
                            return RedirectToAction("MyCart");
                        }
                        else
                        {
                            return RedirectToAction("Index");
                        }                        
                    }
                }
                return View();
            }
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }

        public IActionResult DeleteCartItem(int id)
        {
            Cart cart = SessionHelper.GetProductFromJson<Cart>(HttpContext.Session, "sepet");
            cart.DeleteItem(id);

            //Sepeti güncellemek için
            SessionHelper.SetProductJson(HttpContext.Session, "sepet", cart);

            return RedirectToAction("MyCart");
        }




    }
}
