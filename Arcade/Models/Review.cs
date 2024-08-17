namespace Arcade.Models {
	public class Review {
		public int Id { get; set; }
		public int CustomerId {  get; set; }
		public int GameId { get; set; }
		public string ReviewText {  get; set; }
		public int Rating {  get; set; }
		public string Title {  get; set; }
	}
}
