namespace Arcade.Models {
    public interface IGameRepository {
        void Add(Game game);

        void Update(Game game);

        void Delete(Game game);

        Game GetGameById(int id);

        List<Game> GetAllGames();
    }
}
