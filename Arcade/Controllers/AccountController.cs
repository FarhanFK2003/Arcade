using Arcade.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Arcade.Controllers {
    public class AccountController : Controller {
        private readonly ILogger<AccountController> _logger;

        public AccountController(ILogger<AccountController> logger) {
            _logger = logger;
        }

        public IActionResult Index() {
            var email = Request.Cookies["UserEmail"];
            if(email != null)
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpGet]
        public IActionResult Login() {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string Email, string Password) {
			CustomerRepository cr = new CustomerRepository();
			if (cr.SearchCustomer(Email, Password)) {
                var cookieOptions = new CookieOptions {
                    Expires = DateTimeOffset.Now.AddDays(7),
                    HttpOnly = true,
                    Secure = true
                };
                Response.Cookies.Append("UserEmail", Email, cookieOptions);
                return RedirectToAction("Index", "Home");
            } 
			ViewBag.ErrorMessage = "Invalid email or password.";
			return View();
		}
        [HttpGet]
        public IActionResult Signup() {
            return View();
        }
        [HttpPost]
        public IActionResult Signup(string Email_, string Password_, string Dob_) {
            CustomerRepository cr = new CustomerRepository();
            if(cr.AddCustomer(new Customer { Email=Email_, Password=Password_, Dob=Dob_})) {
                var cookieOptions = new CookieOptions {
                    Expires = DateTimeOffset.Now.AddDays(7),
                    HttpOnly = true,
                    Secure = true 
                };
                Response.Cookies.Append("UserEmail", Email_, cookieOptions);
                return RedirectToAction("Index", "Home");
            }
			ViewBag.ErrorMessage = "Invalid email";
			return View();
		}
    }
}
