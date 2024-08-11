namespace Arcade.Models {
    public interface IOrderRepository {
        
        void PlaceOrder(Order order);

        Order GetOrderById(int id);
    }
}
