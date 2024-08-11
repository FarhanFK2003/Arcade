namespace Arcade.Models {
    public interface ICustomerRepository {
        bool searchEmail(Customer customer);

        bool SearchCustomer(string email, string password);

        bool AddCustomer(Customer customer);
    }
}
