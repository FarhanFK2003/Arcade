namespace Arcade.Models {
    public interface IGameReview {
        void AddReview(GameReview gameReview);
        void UpdateReview(GameReview gameReview);
        void DeleteReview(GameReview gameReview);
        List<GameReview> GetReviewsByGameId(int id);
        List<GameReview> GetReviewsByCustomerId(int id);

    }
}
