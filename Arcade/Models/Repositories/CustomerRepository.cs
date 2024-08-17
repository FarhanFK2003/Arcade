using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using Arcade.Models;
using Arcade.Models.Interfaces;

namespace Arcade.Models.Repositories{
	public class CustomerRepository : IRepository<Customer> {
		private readonly List<Customer> _customers;

		public CustomerRepository() {
			_customers = new List<Customer>();
		}

		public void Add(Customer entity) {
			_customers.Add(entity);

		}

		public Customer GetById(int id) {
			return _customers.FirstOrDefault(p => p.Id == id);
		}

		public IEnumerable<Customer> GetAll() {
			return _customers;
		}

		public void Update(Customer entity) {
			var existingCustomer = _customers.FirstOrDefault(p => p.Id == entity.Id);
			if (existingCustomer != null) {
				existingCustomer.Email = entity.Email;
				existingCustomer.Password = entity.Password;
				existingCustomer.Dob = entity.Dob;
			}
		}

		public void Delete(int id) {
			var Customer = _customers.FirstOrDefault(p => p.Id == id);
			if (Customer != null) {
				_customers.Remove(Customer);
			}
		}
	}
}
