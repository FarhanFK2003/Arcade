using Arcade.Models;
using Arcade.Models.Interfaces;
using Arcade.Models.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Arcade.Controllers {
    public class HomeController : Controller {
        private readonly IRepository<Game> _gameRepository;
		private readonly IRepository<Customer> _customerRepository;
		private readonly IRepository<Review> _reviewRepository;

		public HomeController(IRepository<Game> gameRepository, IRepository<Customer> customerRepository, IRepository<Review> reviewRepository) {
            _gameRepository = gameRepository;
			_customerRepository = customerRepository;
			_reviewRepository = reviewRepository;
		}

        [HttpGet]
        public IActionResult Index() {
            List<Game> games = _gameRepository.GetAll().ToList();
            games = games.OrderBy(g => g.Genre).ToList();
            return View(games);
        }

		[HttpPost]
		public IActionResult Index(int gameId) {
			List<string> cart = new List<string>();
			string currentCart = Request.Cookies["UserCart"];
			if (!string.IsNullOrEmpty(currentCart))
				cart = currentCart.Split(',').ToList();
			if (!cart.Contains(gameId.ToString()))
				cart.Add(gameId.ToString());
			string updatedCart = string.Join(',', cart);

			var cookieOptions = new CookieOptions { Expires = DateTimeOffset.Now.AddDays(7) };
			Response.Cookies.Append("UserCart", updatedCart, cookieOptions);

			List<Game> games = _gameRepository.GetAll().ToList();
			games = games.OrderBy(g => g.Genre).ToList();
			return View(games);
		}

		[HttpGet]
        public IActionResult Search(string query = null) {
            return View();
        }

        [HttpGet]
        public IActionResult Details(int id) {
            GameReviews reviews = new GameReviews();
            Game game = _gameRepository.GetById(id);
            List<Review> reviewsList = _reviewRepository.GetAll().ToList();
            reviews.game = game;
            
            for(int i = 0; i < reviewsList.Count; ++i) {
                if (reviewsList[i].GameId == id) {
                    reviews.reviews.Add(reviewsList[i]);
                    reviews.customers.Add(_customerRepository.GetById(reviewsList[i].CustomerId));
                }
            }
			reviews.customerId = int.Parse(Request.Cookies["userId"]);
			reviews.userEmail = Request.Cookies["userEmail"];
			return View("Details", reviews);
        }
		[HttpPost]
		public IActionResult Details(string strTitle, string strReview, int rating,  int customerId, int id) {
            if (strTitle == null)
                strTitle = "";
            if (strReview == null)
                strReview = "";
            _reviewRepository.Add(new Review { Id = -1, CustomerId = customerId, GameId = id, ReviewText = strReview, Rating = rating, Title=strTitle });
			GameReviews reviews = new GameReviews();
			Game game = _gameRepository.GetById(id);
			List<Review> reviewsList = _reviewRepository.GetAll().ToList();
			reviews.game = game;

			for (int i = 0; i < reviewsList.Count; ++i) {
				if (reviewsList[i].GameId == id) {
					reviews.reviews.Add(reviewsList[i]);
					reviews.customers.Add(_customerRepository.GetById(reviewsList[i].CustomerId));
				}
			}
			reviews.customerId = int.Parse(Request.Cookies["userId"]);
			reviews.userEmail = Request.Cookies["userEmail"];
			return View("Details", reviews);
		}

		[HttpGet]
        public IActionResult Faq() {
            return View();
        }
    }
}
