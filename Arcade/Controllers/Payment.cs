using Microsoft.AspNetCore.Mvc;

namespace Arcade.Controllers {
	public class Payment : Controller {

		[HttpGet]
		public IActionResult End(string query = null) {
			return View();
		}

		[HttpGet]
		public IActionResult Checkout(string query = null) {
			return View();
		}
	}
}
