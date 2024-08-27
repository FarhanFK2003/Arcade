namespace Arcade.Models {
	public class GameReviews {
		public Game game { get; set; }
		public List<Customer> customers { get; set; }
		public List<Review> reviews { get; set; }
		public string userEmail { get; set; }

		public string customerId { get; set; }

		public GameReviews() {
			customers = new List<Customer>();
			reviews = new List<Review>();

		}

	}
}
