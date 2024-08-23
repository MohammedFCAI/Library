using Microsoft.AspNetCore.SignalR;

namespace LibrarySystem
{
    public sealed class ChatHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("ReceiveMessage", $"{Context.ConnectionId} has joined");
        }


        public Task SendMessage(string user, string message)
        {
            return Clients.All.SendAsync("ReceiveOne", user, message);
        }
    }
}
