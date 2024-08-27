using Arcade.Models.Interfaces;
using System.Data.SqlClient;

using Arcade.Models;

namespace Arcade.Models.Repositories {
	public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class {
		private readonly string _connectionString;

		public GenericRepository(string connectionString) {
			_connectionString = connectionString;
		}

		public void Add(TEntity entity) {
            try {
				using (var connection = new SqlConnection(_connectionString)) {
					connection.Open();
					var tableName = typeof(TEntity).Name + 's';
					var properties = typeof(TEntity).GetProperties().Where(p => p.Name != "Id");
					if (tableName == "UsersImages")
						properties = typeof(TEntity).GetProperties();

                    var columnNames = string.Join(",", properties.Select(p => p.Name));
					var parameterNames = string.Join(",", properties.Select(p => "@" + p.Name));

					var query = $"INSERT INTO {tableName} ({columnNames}) VALUES ({parameterNames});";

					using (var command = new SqlCommand(query, connection)) {
						foreach (var property in properties) {
							command.Parameters.AddWithValue("@" + property.Name, property.GetValue(entity));
						}

						command.ExecuteNonQuery();
					}
				}
			}
			catch (SqlException ex) { }
		}

        public TEntity GetById(object id) {
            using (var connection = new SqlConnection(_connectionString)) {
                connection.Open();
                var tableName = typeof(TEntity).Name + 's';
                var primaryKey = "Id";

                var query = $"SELECT * FROM {tableName} WHERE {primaryKey} = @Id;";
                using (var command = new SqlCommand(query, connection)) {
                    if (id is int) {
                        command.Parameters.AddWithValue("@Id", (int)id);
                    }
                    else if (id is string) {
                        command.Parameters.AddWithValue("@Id", (string)id);
                    }
                    else {
                        throw new ArgumentException("Invalid ID type. Only int and string are supported.");
                    }

                    var reader = command.ExecuteReader();
                    if (reader.Read()) {
                        return MapReaderToObject(reader);
                    }

                    return null;
                }
            }
        }


        public IEnumerable<TEntity> GetAll() {
			using (var connection = new SqlConnection(_connectionString)) {
				connection.Open();
				var tableName = typeof(TEntity).Name + 's';

				var query = $"SELECT * FROM {tableName};";
				using (var command = new SqlCommand(query, connection)) {
					var reader = command.ExecuteReader();
					var entities = new List<TEntity>();
					while (reader.Read()) {
						entities.Add(MapReaderToObject(reader));
					}
					return entities;
				}
			}
		}

		public void Update(TEntity entity) {
			using (var connection = new SqlConnection(_connectionString)) {
				connection.Open();
				var tableName = typeof(TEntity).Name + 's';
				var primaryKey = "Id";

				var properties = typeof(TEntity).GetProperties().Where(p => p.Name != primaryKey);

				var setClause = string.Join(",", properties.Select(p => $"{p.Name} = @{p.Name}"));
				var query = $"UPDATE {tableName} SET {setClause} WHERE {primaryKey} = @{primaryKey};";

				using (var command = new SqlCommand(query, connection)) {
					foreach (var property in properties) {
						command.Parameters.AddWithValue("@" + property.Name, property.GetValue(entity));
					}
					command.Parameters.AddWithValue("@" + primaryKey, typeof(TEntity).GetProperty(primaryKey).GetValue(entity));

					command.ExecuteNonQuery();
				}
			}
		}

		public void Delete(int id) {
			using (var connection = new SqlConnection(_connectionString)) {
				connection.Open();
				var tableName = typeof(TEntity).Name + 's';
				var primaryKey = "Id";

				var query = $"DELETE FROM {tableName} WHERE {primaryKey} = @Id;";
				using (var command = new SqlCommand(query, connection)) {
					command.Parameters.AddWithValue("@Id", id);
					command.ExecuteNonQuery();
				}
			}
		}

		private TEntity MapReaderToObject(SqlDataReader reader) {
			var entity = Activator.CreateInstance<TEntity>();
			foreach (var property in typeof(TEntity).GetProperties()) {
				if (!reader.IsDBNull(reader.GetOrdinal(property.Name))) {
					property.SetValue(entity, reader[property.Name]);
				}
			}
			return entity;
		}


	}

}
