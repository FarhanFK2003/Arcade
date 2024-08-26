using Arcade.Models;
using Arcade.Models.Interfaces;
using Arcade.Models.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Arcade.Controllers {
    public class AccountController : Controller {
        private readonly ILogger<AccountController> _logger;
        private readonly IRepository<Customer> _customerRepository;
        
        public AccountController(IRepository<Customer> customerRepository) {
            _customerRepository = customerRepository;
        }
        [HttpGet]
        public IActionResult Index() {
            var email = Request.Cookies["UserEmail"];
            if (email != null) {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Logout() {
            Response.Cookies.Delete("UserEmail");
            Response.Cookies.Delete("UserId");
            Response.Cookies.Delete("UserCart");

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Login() {
            return View();
        }

		private int GetCustomerId(string Email) {
			List<Customer> lst = _customerRepository.GetAll().ToList();
			int id = -1;
			for (int i = 0; i < lst.Count(); ++i)
				if (lst[i].Email == Email) {
					id = lst[i].Id;
					break;
				}
			return id;
		}

		[HttpPost]
        public IActionResult Login(string Email, string Password) {
            List<Customer> lst = _customerRepository.GetAll().ToList();
            bool ok = false;
            for (int i = 0; i < lst.Count(); ++i)
                if (lst[i].Email == Email && lst[i].Password == Password) {
                    ok = true;
                    break;
                }

            if (ok) {
                var cookieOptions = new CookieOptions { Expires = DateTimeOffset.Now.AddDays(7) };
                Response.Cookies.Append("UserEmail", Email, cookieOptions);
                Response.Cookies.Append("UserId", GetCustomerId(Email).ToString(), cookieOptions);
				Response.Cookies.Delete("UserCart");
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
        public IActionResult Signup(string Email, string Password, string Dob) {
            List<Customer> lst = _customerRepository.GetAll().ToList();
            bool ok = false;
            for (int i = 0; i < lst.Count(); ++i)
                if (lst[i].Email == Email) {
                    ok = true;
                    break;
                }
            if (!ok) {
                var cookieOptions = new CookieOptions { Expires = DateTimeOffset.Now.AddDays(7) };
                _customerRepository.Add(new Customer { Email = Email, Password = Password, Dob = Dob, ImgPath=@"/images/u2.png"});
                Response.Cookies.Append("UserEmail", Email, cookieOptions);
				Response.Cookies.Append("UserId", GetCustomerId(Email).ToString(), cookieOptions);
				Response.Cookies.Delete("CustomerCart");
                return RedirectToAction("Index", "Home");
            }
            ViewBag.ErrorMessage = "Invalid email";
            return View();
        }
    }
}
