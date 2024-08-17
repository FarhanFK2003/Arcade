using Arcade.Models.Interfaces;
using System.Data.SqlClient;

namespace Arcade.Models.Repositories
{
	public class GameRepository : IRepository<Game> {
		private readonly List<Game> _Games;

		public GameRepository() {
			_Games = new List<Game>();
		}

		public void Add(Game entity) {
			_Games.Add(entity);
		}

		public Game GetById(int id) {
			return _Games.FirstOrDefault(p => p.Id == id);
		}

		public IEnumerable<Game> GetAll() {
			return _Games;
		}

		public void Update(Game entity) {
			var existingGame = _Games.FirstOrDefault(p => p.Id == entity.Id);
			if (existingGame != null) {
				existingGame.Name = entity.Name;
				existingGame.Price = entity.Price;
				existingGame.Genre = entity.Genre;
				existingGame.ImgPath = entity.ImgPath;
			}
		}

		public void Delete(int id) {
			var Game = _Games.FirstOrDefault(p => p.Id == id);
			if (Game != null) {
				_Games.Remove(Game);
			}
		}
	}
}
