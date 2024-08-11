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
            return View();
        }

        [HttpGet]
        public IActionResult Login() {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string Email, string Password) {
			CustomerRepository cr = new CustomerRepository();
			if (cr.SearchCustomer(Email, Password)) 
				return RedirectToAction("Index", "Home");
			ViewBag.ErrorMessage = "Invalid email or password.";
			return View();
		}
        [HttpGet]
        public IActionResult Signup() {
            return View();
        }
        [HttpPost]
        public IActionResult Signup(string Email, string Password, string Dob) {
            CustomerRepository cr = new CustomerRepository();
            cr.AddCustomer(new Customer(Email, Password, Dob));
            return View("Index");
        }
    }
}
