using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Project.BLL.Repositories.CategoryRepository;
using Project.BLL.Repositories.ProductRepository;
using Project.Common;
using Project.Entity.Entity;
using Project.WEB.Areas.Admin.Models;
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

            //Bu method sepete stoktan fazla ürün eklenmemesi için oluşturulmuştur.
            var result=StockCheck.GetStockCheck(product.UnitsInStock, product.Id);

            if (result)
            {
                cartSession.AddItem(cartItem);
                SessionHelper.SetProductJson(HttpContext.Session, "sepet", cartSession);
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> MyCart()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await userManager.GetUserAsync(User);
                ViewBag.Coupon = user.CouponCode;

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

        //Kupon kodu girilmesi durumunda indirim uygulanması için yapılmıştır.
        [HttpPost]
        public async Task<IActionResult> MyCart(string couponCode)
        {
            var user = await userManager.GetUserAsync(User);

            if (couponCode==user.CouponCode && user.CouponUsing!=true)
            {
                Cart sepet = SessionHelper.GetProductFromJson<Cart>(HttpContext.Session, "sepet");

                foreach (CartItem item in sepet.Mycart)
                {
                    item.UnitPrice = item.UnitPrice *0.9m;
                }
                user.CouponUsing = true;
                await userManager.UpdateAsync(user);
                
                SessionHelper.SetProductJson(HttpContext.Session, "sepet", sepet);
                return RedirectToAction("MyCart");
            }
            else
            {
                return RedirectToAction("MyCart");
            }
        }


        public IActionResult DecreaseCart(int id)
        {
            Cart cart = SessionHelper.GetProductFromJson<Cart>(HttpContext.Session, "sepet");
            cart.DecreaseItem(id);

            //Sepeti güncellemek için
            SessionHelper.SetProductJson(HttpContext.Session, "sepet", cart);

            if (cart.Mycart.Count == 0)
            {
                SessionHelper.RemoveSession(HttpContext.Session, "sepet");
            }

            return RedirectToAction("MyCart");
        }

        public IActionResult IncreaseCart(int id)
        {
            Cart cart = SessionHelper.GetProductFromJson<Cart>(HttpContext.Session, "sepet");
            
            var product = productRepository.GetById(id);
            //Bu method sepete stoktan fazla ürün eklenmemesi için oluşturulmuştur.
            var result = StockCheck.GetStockCheck(product.UnitsInStock, product.Id);

            if (result)
            {
                cart.IncreaseItem(id);
                SessionHelper.SetProductJson(HttpContext.Session, "sepet", cart);
                return RedirectToAction("MyCart");
            }
            else
            {
                return RedirectToAction("MyCart");
            }
        }


        public IActionResult Register()
        {
            TempData["result"] = null;
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
                Random rnd = new Random();
                int coupon = rnd.Next(10, 100);
                newUser.CouponCode = "WELCOME" + coupon.ToString();
                var result = await userManager.CreateAsync(newUser, registerUser.Password);
               
                if (result.Succeeded)
                {
                    var registerToken = userManager.GenerateEmailConfirmationTokenAsync(newUser).Result;
                    string code = Guid.NewGuid().ToString();
                    TransformHelper.transformObject = code;

                    //TempData burada neden çalışmıyor??
                    //TempData["Code"] = code;
                    TempData["RegisterToken"] = registerToken;

                    var callBackUrl = Url.Action("Confirmation", "Home", new { id = newUser.Id, code = code }, protocol: Request.Scheme);

                    MailSender.SendEmail(registerUser.Email, "Register", $"Merhaba! {registerUser.Username} kayıt işleminiz başarılı şekilde oluşturuldu! Yeni üyelere özel %10 hoşgeldiniz indirim kuponunuz {newUser.CouponCode} kodu ile tanımlandı. Üyeliği tamamlamak için linke tıklayın: <a href=\"" + callBackUrl + "\">link</a>");

                    //MailSender.SendEmail(registerUser.Email, "Register", $"Merhaba {registerUser.Username}! kayıt işleminiz başarılı şekilde oluşturuldu! Üyeliği tamamlamak için linke tıklayın https://localhost:5001/home/confirmation/" + newUser.Id + "/" + registerToken);
                    
                    TempData["result"] = $"{newUser.Email} adresine aktivasyon maili gönderdik. Üyeliğinizi aktif hale getirmek için ilgili linki tıklayın.";
                    return View(registerUser);
                }
                else
                {
                    return View(registerUser);
                }
            }
            return View(registerUser);
        }

        public async Task<IActionResult> Confirmation(string id, string code)
        {
            if (id!=null && code == (string)TransformHelper.transformObject)
            {
                var user = await userManager.FindByIdAsync(id);
                var confirm = await userManager.ConfirmEmailAsync(user, TempData["RegisterToken"] as string);
                if (confirm.Succeeded)
                {
                    return RedirectToAction("Login");
                }
            }
            return RedirectToAction("Index");
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

            if (cart.Mycart.Count==0)
            {
                SessionHelper.RemoveSession(HttpContext.Session, "sepet");
            }

            return RedirectToAction("MyCart");
        }

    }
}
