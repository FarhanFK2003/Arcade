namespace Arcade.Models.Interfaces {
	public interface IRepository<TEntity> {
		void Add(TEntity entity);
		TEntity GetById(object id);
		IEnumerable<TEntity> GetAll();
		void Update(TEntity entity);
		void Delete(int id);
	}
}
