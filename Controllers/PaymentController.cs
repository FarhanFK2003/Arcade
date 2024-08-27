using Arcade.Models;
using Arcade.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Arcade.Extensions;

namespace Arcade.Controllers {
	public class PaymentController : Controller {

		private readonly IRepository<Game> _gameRepository;
		private readonly IRepository<Customer> _customerRepository;
		private readonly IRepository<Review> _reviewRepository;
		private readonly IWebHostEnvironment Environment;
		public PaymentController(IRepository<Game> gameRepository, IRepository<Customer> customerRepository,
			IRepository<Review> reviewRepository, IWebHostEnvironment environment) {
			_gameRepository = gameRepository;
			_customerRepository = customerRepository;
			_reviewRepository = reviewRepository;
			Environment = environment;
		}

		[HttpGet]
		public IActionResult End(string query = null) {
			return View();
		}

		[HttpGet]
		public IActionResult Checkout(string query = null) {
			return View();
		}

		[HttpGet]
		public IActionResult GetCart() {
			var cartGameIds = HttpContext.Session.Get<List<int>>("CartGameIds") ?? new List<int>();
			List<Game> games = new List<Game>();
			for (int i = 0; i < cartGameIds.Count; ++i)
				games.Add(_gameRepository.GetById(cartGameIds[i]));
			return new JsonResult(games);
		}

		[HttpPost]
		public IActionResult RemoveFromCart(int gameId) {
            var cartGameIds = HttpContext.Session.Get<List<int>>("CartGameIds") ?? new List<int>();
			if (cartGameIds.Contains(gameId))
				cartGameIds.Remove(gameId);
            HttpContext.Session.Set("CartGameIds", cartGameIds);
            return Json(true);
        }
	}
}
