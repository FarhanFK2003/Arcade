using Arcade.Models;
using Arcade.Models.Interfaces;
using Arcade.Models.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Arcade.Controllers {
    public class AccountController : Controller {
        private readonly ILogger<AccountController> _logger;
        private readonly IRepository<Customer> _customerRepository;
        IRepository<Customer> customerRepository = new GenericRepository<Customer>(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ArcadeDB;");

        public AccountController(IRepository<Customer> customerRepository) {
            _customerRepository = customerRepository;
        }

        public IActionResult Index() {
            var email = Request.Cookies["UserEmail"];
            if (email != null)
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpGet]
        public IActionResult Login() {
            return View();
        }

		private int GetCustomerId(string Email) {
			List<Customer> lst = customerRepository.GetAll().ToList();
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
            List<Customer> lst = customerRepository.GetAll().ToList();
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
            List<Customer> lst = customerRepository.GetAll().ToList();
            bool ok = false;
            for (int i = 0; i < lst.Count(); ++i)
                if (lst[i].Email == Email) {
                    ok = true;
                    break;
                }
            if (!ok) {
                var cookieOptions = new CookieOptions { Expires = DateTimeOffset.Now.AddDays(7) };
			    customerRepository.Add(new Customer { Email = Email, Password = Password, Dob = Dob });
                Response.Cookies.Append("UserEmail", Email, cookieOptions);
				Response.Cookies.Append("UserId", GetCustomerId(Email).ToString(), cookieOptions);
                
				return RedirectToAction("Index", "Home");
            }
            ViewBag.ErrorMessage = "Invalid email";
            return View();
        }
    }
}
