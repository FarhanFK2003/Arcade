using System.Data.Common;

namespace Arcade.Models {
    public class OrderRepository : IOrderRepository{
        private string connectionString;
        public OrderRepository() {
            connectionString = "";
        }

        public void PlaceOrder(Order order) {
            return;
        }

        public Order GetOrderById(int id) {
            return new Order();
        }
    }
}
