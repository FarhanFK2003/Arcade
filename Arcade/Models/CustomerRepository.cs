using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
namespace Arcade.Models {
    public class CustomerRepository {
        public string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ArcadeDB;Integrated Security=True;";
        
        public CustomerRepository() { }

        public bool searchEmail(Customer customer) {
            string query = "Select * from Customer where email = @e";
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@e", customer.Email);
            SqlDataReader dr = cmd.ExecuteReader();
            dr.Read();
            if (dr.HasRows) {
                con.Close();
                return true;
            }
            return false;
        }

        public bool SearchCustomer(string email, string password) {
            string query = "Select * from Customer where email = @e and password = @p";
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@e", email);
            cmd.Parameters.AddWithValue("@p", password);
            SqlDataReader dr = cmd.ExecuteReader();
            dr.Read();
            if (dr.HasRows) {
                con.Close();
                return true;
            }
            return false;
        }

        public void AddCustomer(Customer customer) {
            if (searchEmail(customer))
                return;
            string query = "Insert into Customer Values(@e, @p, @d)";
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@e", customer.Email);
            cmd.Parameters.AddWithValue("@p", customer.Password);
            cmd.Parameters.AddWithValue("@d", customer.Dob);

            cmd.ExecuteNonQuery();

            con.Close();
        }
    }
}
