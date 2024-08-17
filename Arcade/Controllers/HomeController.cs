using Arcade.Models;
using Arcade.Models.Interfaces;
using Arcade.Models.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Arcade.Controllers {
    public class HomeController : Controller {
        private readonly IRepository<Game> _gameRepository;
		private readonly IRepository<Game> _customerRepository;
		private readonly IRepository<Game> _reviewRepository;
		IRepository<Game> gameRepository = new GenericRepository<Game>(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ArcadeDB;");
		IRepository<Customer> customerRepository = new GenericRepository<Customer>(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ArcadeDB;");
		IRepository<Review> reviewRepository = new GenericRepository<Review>(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ArcadeDB;");
		public HomeController(IRepository<Game> gameRepository, IRepository<Game> customerRepository, IRepository<Game> reviewRepository) {
            _gameRepository = gameRepository;
			_customerRepository = customerRepository;
			_reviewRepository = reviewRepository;
		}

        [HttpGet]
        public IActionResult Index() {
            List<Game> games = gameRepository.GetAll().ToList();
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
            Game game = gameRepository.GetById(id);
            List<Review> reviewsList = reviewRepository.GetAll().ToList();
            reviews.game = game;
            
            for(int i = 0; i < reviewsList.Count; ++i) {
                if (reviewsList[i].GameId == id) {
                    reviews.reviews.Add(reviewsList[i]);
                    reviews.customers.Add(customerRepository.GetById(reviewsList[i].CustomerId));
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
            reviewRepository.Add(new Review { Id = -1, CustomerId = customerId, GameId = id, ReviewText = strReview, Rating = rating, Title=strTitle });
			GameReviews reviews = new GameReviews();
			Game game = gameRepository.GetById(id);
			List<Review> reviewsList = reviewRepository.GetAll().ToList();
			reviews.game = game;

			for (int i = 0; i < reviewsList.Count; ++i) {
				if (reviewsList[i].GameId == id) {
					reviews.reviews.Add(reviewsList[i]);
					reviews.customers.Add(customerRepository.GetById(reviewsList[i].CustomerId));
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
