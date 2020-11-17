using System;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Chat_Application.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace Chat_Application.Hubs
{
    public class ChatHub : Hub
    {
        string name, conId;

        private readonly SignInManager<Chat_ApplicationUser> signInManager;
        private readonly UserManager<Chat_ApplicationUser> userManager;

        public ChatHub(SignInManager<Chat_ApplicationUser> _signInManager, UserManager<Chat_ApplicationUser> _userManager)
        {
            signInManager = _signInManager;
            userManager = _userManager;
        }

        public Task SendMessage(string sender, string _conId, string msg)
        {
            return Clients.Client(_conId).SendAsync("ReceiveMessage", sender, msg);
        }

        public override async Task OnConnectedAsync()
        {
            conId = Context.ConnectionId;

            await Clients.Caller.SendAsync("UserConfig", name, conId);
            await Clients.All.SendAsync("UserConnected", name, conId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception e)
        {
            await Clients.All.SendAsync("UserDisconnected", Context.ConnectionId);
            await base.OnDisconnectedAsync(e);
        }
    }
}