namespace Arcade.Models {
    public class GameReviewRepository {
        private string connectionString;

        public GameReviewRepository() {
            connectionString = "";
        }

        public void AddReview(GameReview gameReview) {
            return;
        }
        public void UpdateReview(GameReview gameReview) {
            return;
        }
        public void DeleteReview(GameReview gameReview) {
            return;
        }
        public List<GameReview> GetReviewsByGameId(int id) {
            return new List<GameReview>();
        }
        public List<GameReview> GetReviewsByCustomerId(int id) {
            return new List<GameReview>();
        }
    }
}
