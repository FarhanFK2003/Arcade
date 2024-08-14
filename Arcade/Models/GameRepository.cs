using System.Data.SqlClient;

namespace Arcade.Models {
    public class GameRepository : IGameRepository {
        private string connectionString;

        public GameRepository() {
            connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ArcadeDB;";
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

        public List<Game> GetAllGames() {
            string query = "Select * from Games order by Genre";
            List<Game> ret = new List<Game>();
            using (SqlConnection con = new SqlConnection(connectionString)) {
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader()) {
                    while (dr.Read()) {
                        string name = dr["Name"].ToString();
                        string genre = dr["Genre"].ToString();
                        string imgPath = dr["ImgPath"].ToString();
                        double price = (double)dr["Price"];
                        ret.Add(new Game {Name=name, Genre=genre, ImgPath=imgPath, Price=price});
                    }
                }
            }
            return ret;
        }
    }
}
