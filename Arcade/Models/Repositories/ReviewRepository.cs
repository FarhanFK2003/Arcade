using Arcade.Models.Interfaces;
using Arcade.Models;

namespace Arcade.Models.Repositories {
	public class ReviewRepository : IRepository<Review> {
		private readonly List<Review> _Reviews;
		public ReviewRepository() {
			_Reviews = new List<Review>();
		}

		public void Add(Review entity) {
			_Reviews.Add(entity);
		}

		public Review GetById(int id) {
			return _Reviews.FirstOrDefault(p => p.Id == id);
		}

		public IEnumerable<Review> GetAll() {
			return _Reviews;
		}

		public void Update(Review entity) {
			var existingReview = _Reviews.FirstOrDefault(p => p.Id == entity.Id);
			if (existingReview != null) {
				existingReview.CustomerId = entity.CustomerId;
				existingReview.GameId = entity.GameId;
				existingReview.ReviewText = entity.ReviewText;
				existingReview.Rating = entity.Rating;
				existingReview.Title = entity.Title;
			}
		}

		public void Delete(int id) {
			var Review = _Reviews.FirstOrDefault(p => p.Id == id);
			if (Review != null) {
				_Reviews.Remove(Review);
			}
		}
	}
}
