using Arcade.Models;
using Arcade.Models.Interfaces;
using Arcade.Models.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Newtonsoft.Json;
using Arcade.Extensions;

namespace Arcade.Controllers {
    public class HomeController : Controller {
        private readonly IRepository<Game> _gameRepository;
		private readonly IRepository<Customer> _customerRepository;
		private readonly IRepository<Review> _reviewRepository;
        private readonly IWebHostEnvironment Environment;
        public HomeController(IRepository<Game> gameRepository, IRepository<Customer> customerRepository, 
            IRepository<Review> reviewRepository, IWebHostEnvironment environment) {
            _gameRepository = gameRepository;
			_customerRepository = customerRepository;
			_reviewRepository = reviewRepository;
            Environment = environment;
		}

        [HttpGet]
        public IActionResult Index(string searchQuery = "") {
            
            return View("Index", searchQuery);
        }

        [HttpGet]
        public JsonResult GetGamesList(string searchQuery = "") {
            List<Game> games = _gameRepository.GetAll().ToList();
            games = games
                .Where(g=> string.IsNullOrEmpty(searchQuery) || g.Name.ToLower().Contains(searchQuery.ToLower()))
                .OrderBy(g => g.Genre).ToList();
            return Json(games);
        }

        [HttpPost]
		public IActionResult AddGameToCart(int gameId) {
			var cartGameIds = HttpContext.Session.Get<List<int>>("CartGameIds") ?? new List<int>();
			if (!cartGameIds.Contains(gameId)) 
				cartGameIds.Add(gameId);
			HttpContext.Session.Set("CartGameIds", cartGameIds);
			return Json(new { success = true });
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
			
			return Json(new { Id = -1, customerId = customerId, gameId = id, reviewText = strReview, rating = rating, title = strTitle
            , imgPath= _customerRepository.GetById(customerId).ImgPath, email= _customerRepository.GetById(customerId).Email
			});
		}
        [HttpGet]
        public IActionResult GetImgPath() {
            int customerId = int.Parse(Request.Cookies["UserId"]);
            Customer customer = _customerRepository.GetById(customerId);

            if (customer != null && !string.IsNullOrEmpty(customer.ImgPath)) {
                return Json(customer.ImgPath);
            }

            return Json("/images/u2.png");
        }

        [HttpPost]
        public IActionResult UploadPfp(IFormFile file) {
            string wwwPath = this.Environment.WebRootPath;
            string path = Path.Combine(wwwPath, "images");
            int customerId = int.Parse(Request.Cookies["UserId"]);
            Customer customer = _customerRepository.GetById(customerId);
            var fileNamePrevious = Path.GetFileName(file.FileName);
            var fileName = $"{customer.Email}{Guid.NewGuid()}{Path.GetExtension(fileNamePrevious)}";
            var pathWithFileName = Path.Combine(path, fileName);
            using (FileStream stream = new FileStream(pathWithFileName, FileMode.Create)) {
                file.CopyTo(stream);
                ViewBag.Message = "file uploaded successfully";
                ViewBag.FileName = fileName;
            }
            customer.ImgPath = @$"/images/{fileName}";
            _customerRepository.Update(customer);
            return Json(new { success = true, fileName = customer.ImgPath });
        }

		[HttpGet]
        public IActionResult Faq() {
            return View();
        }
    }
}
