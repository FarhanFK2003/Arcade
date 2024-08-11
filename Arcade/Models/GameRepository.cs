namespace Arcade.Models {
    public class GameRepository : IGameRepository {
        private string connectionString;

        public GameRepository() {
            connectionString = "";
        }
        public void Add(Game game) {
            return;
        }
        public void Update(Game game) {
            return;
        }

        public void Delete(Game game) {
            return;
        }

        public Game GetGameById(int id) {
            return new Game();
        }
    }
}
