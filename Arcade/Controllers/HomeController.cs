using Microsoft.AspNetCore.Mvc;

namespace Arcade.Controllers {
    public class HomeController : Controller {
        [HttpGet]
        public IActionResult Index() {
            return View();
        }

        [HttpGet]
        public IActionResult Search(string query = null) {
            return View();
        }

        [HttpGet]
		public IActionResult Details(string gameName = null) {
			return View();
		}
	}
}
