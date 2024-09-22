using Arcade.Models;
using Arcade.Models.Interfaces;
using Arcade.Models.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Newtonsoft.Json;
using Arcade.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;


namespace Arcade.Controllers {
    public class HomeController : Controller {
        private readonly IRepository<Game> _gameRepository;
		private readonly IRepository<Customer> _customerRepository;
		private readonly IRepository<Review> _reviewRepository;
        private readonly IRepository<UsersImage> _usersImageRepository;
        private readonly IWebHostEnvironment Environment;
        private readonly IRepository<CustomerGame> _customerGameRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRepository<Faq> _faqRepository;
        public HomeController(IRepository<Game> gameRepository, IRepository<Customer> customerRepository, IRepository<UsersImage> usersImageRepository,
        IRepository<Review> reviewRepository, IRepository<Faq> faqRepository,
        IWebHostEnvironment environment, UserManager<IdentityUser> userManager, IRepository<CustomerGame> customerGameRepository) {
            _gameRepository = gameRepository;
			_customerRepository = customerRepository;
			_reviewRepository = reviewRepository;
            Environment = environment;
            _userManager = userManager;
            _usersImageRepository = usersImageRepository;
            _customerGameRepository = customerGameRepository;
            _faqRepository = faqRepository;
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

        [HttpGet]
        public JsonResult GetBoughtGamesOfCurrentUser() {
            string customerId = Request.Cookies["UserId"];
            List<CustomerGame> customerGames = _customerGameRepository.GetAll().ToList();
            List<CustomerGame> games = customerGames.Where(cg=> cg.CustomerId == customerId).ToList();
            return Json(games);
        }

        [HttpPost]
		public IActionResult AddGameToCart(int gameId) {
			var cartGameIds = HttpContext.Session.Get<List<int>>("CartGameIds") ?? new List<int>();
			if (!cartGameIds.Contains(gameId)) 
				cartGameIds.Add(gameId);
            else
				return StatusCode(409, new { success = false, message = "Game is already in the cart." });
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
                    var user = _userManager.FindByIdAsync(reviewsList[i].CustomerId).Result;
                    var userImage = _usersImageRepository.GetById(user.Id);
                    reviews.customers.Add(new Customer { Email=user.Email, Id=user.Id, ImgPath=userImage.ImgPath});
                }
            }
			reviews.customerId = Request.Cookies["userId"];
			reviews.userEmail = Request.Cookies["userEmail"];
			return View("Details", reviews);
        }

		[HttpPost]
		public IActionResult Details(string strTitle, string strReview, int rating,  string customerId, int id) {
            
            if (strTitle == null)
                strTitle = "";
            if (strReview == null)
                strReview = "";
            _reviewRepository.Add(new Review { Id = -1, CustomerId = customerId, GameId = id, ReviewText = strReview, Rating = rating, Title=strTitle });

            var user = _userManager.FindByIdAsync(customerId).Result;
            var userImage = _usersImageRepository.GetById(customerId);
            return Json(new { Id = -1, customerId = customerId, gameId = id, reviewText = strReview, rating = rating, title = strTitle
            , imgPath= userImage.ImgPath, email= user.Email
			});
		}
        [HttpGet]
        public IActionResult GetImgPath() {
            string customerId = Request.Cookies["UserId"];
            var userImage = _usersImageRepository.GetById(customerId);

            if (userImage != null && !string.IsNullOrEmpty(userImage.ImgPath)) {
                return Json(userImage.ImgPath);
            }

            return Json("/images/u2.png");
        }

        [HttpPost]
        public IActionResult UploadPfp(IFormFile file) {
            string wwwPath = this.Environment.WebRootPath;
            string path = Path.Combine(wwwPath, "images");
            string customerId = Request.Cookies["UserId"];
            var user = _userManager.FindByIdAsync(customerId).Result;

            var fileNamePrevious = Path.GetFileName(file.FileName);
            var fileName = $"{user.Email}{Guid.NewGuid()}{Path.GetExtension(fileNamePrevious)}";
            var pathWithFileName = Path.Combine(path, fileName);
            using (FileStream stream = new FileStream(pathWithFileName, FileMode.Create)) {
                file.CopyTo(stream);
                ViewBag.Message = "file uploaded successfully";
                ViewBag.FileName = fileName;
            }
            var userImage = new UsersImage{ Id=customerId, ImgPath= @$"/images/{fileName}"};
            _usersImageRepository.Update(userImage);
            return Json(new { success = true, fileName = userImage.ImgPath });
        }

		[HttpGet]
        public IActionResult Faq() {
            return View();
        }

        public IActionResult GetFaqs() {
            List<Faq> faqs = new List<Faq>();
            faqs = _faqRepository.GetAll().ToList();
            return Json(faqs);
        }
    }
}
