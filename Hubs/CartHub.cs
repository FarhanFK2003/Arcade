using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Arcade.Hubs {
    public class CartHub : Hub {
        public async Task NotifyCartUpdated(string message) {
            await Clients.All.SendAsync("CartUpdated",message);
        }
    }
}