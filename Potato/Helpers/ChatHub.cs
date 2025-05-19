using Microsoft.AspNetCore.SignalR;

namespace Potato.Helpers
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string senderUserName, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", senderUserName, message);
        }
    }
}
