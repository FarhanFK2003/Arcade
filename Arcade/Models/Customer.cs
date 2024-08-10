namespace Arcade.Models {
    public class Customer {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Dob { get; set; }

        public Customer(string email, string password, string dob) {
            Email = email;
            Password = password;
            Dob = dob;
        }
    }
}
