using System;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ChatApplication.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace ChatApplication.Hubs
{
    public class ChatHub : Hub
    {
        //private readonly SignInManager<Chat_ApplicationUser> signInManager;
        //private readonly UserManager<Chat_ApplicationUser> userManager;

        //public ChatHub(SignInManager<Chat_ApplicationUser> _signInManager, UserManager<Chat_ApplicationUser> _userManager)
        //{
        //    signInManager = _signInManager;
        //    userManager = _userManager;
        //}

        public void ConfigureUser(string username)
        {
            string conId = Context.ConnectionId;
            //DB => username, conId
        }

        public Task SendMessage(string _conId, string from, string msg)
        {
            return Clients.Client(_conId).SendAsync("ReceiveMessage", from, msg);
        }

        public Task RegisterUser(string email, string conId)
        {
            return Clients.Others.SendAsync("NewUserJoined", email, conId);
        }

        public override async Task OnConnectedAsync()
        {
            string conId = Context.ConnectionId;
            await Clients.Caller.SendAsync("SetConnectionId", conId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception e)
        {
            await Clients.All.SendAsync("UserDisconnected", Context.ConnectionId);
            await base.OnDisconnectedAsync(e);
        }
    }
}