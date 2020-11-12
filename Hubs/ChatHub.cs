using System;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Chat_Application.Hubs
{
    public class ChatHub : Hub
    {
        public Task SendMessageCaller(string from, string to, string message)
        {
            return Clients.Caller.SendAsync("RecieveMessageCaller", from, to, message);
        }

        public Task SendMessageTo(string from, string to, string message)
        {
            return Clients.Client(to).SendAsync("RecieveMessageTo", from, to, message);
        }

        public Task SendMessageAll(string from, string to, string message)
        {
            return Clients.All.SendAsync("RecieveMessageAll", from, to, message);
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync("UserConfig", Context.ConnectionId);
            await Clients.All.SendAsync("UserConnected", Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception e)
        {
            await Clients.All.SendAsync("UserDisconnected", Context.ConnectionId);
            await base.OnDisconnectedAsync(e);
        }
    }
}