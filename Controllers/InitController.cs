using Microsoft.AspNetCore.Mvc;

namespace Arcade_.Controllers {
    public class InitController : Controller {
        public IActionResult Index() {
            return View();
        }
    }
}
