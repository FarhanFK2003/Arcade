namespace Arcade.Models {
    public class GameReview {

        public int ReviewId { get; set; }
        public int GameId { get; set; }
        public int CustomerId { get; set; }
        public int Rating {  get; set; }
        public string Comment { get; set; }

    }
}
