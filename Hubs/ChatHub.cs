using System;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Chat_Application.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace Chat_Application.Hubs
{
    public class ChatHub : Hub
    {
        //public object User { get; private set; }
        private readonly SignInManager<Chat_ApplicationUser> _signInManager;
        private readonly UserManager<Chat_ApplicationUser> _userManager;

        public ChatHub(SignInManager<Chat_ApplicationUser> signInManager, UserManager<Chat_ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

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
            //if (_signInManager.IsSignedIn(User))
            //    Console.WriteLine(_userManager.GetUserName(User));
            //Clients.All.SendAsync("fxn");
            //ViewBag.username = _userManager.GetUserName(HttpContext.User);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception e)
        {
            await Clients.All.SendAsync("UserDisconnected", Context.ConnectionId);
            await base.OnDisconnectedAsync(e);
        }
    }
}