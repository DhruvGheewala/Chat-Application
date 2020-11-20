using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ChatApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using ChatApplication.Areas.Identity.Data;
using ChatApplication.Data;
using Microsoft.EntityFrameworkCore;

namespace ChatApplication.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        
        private readonly ChatApplicationContext _context;
        private readonly UserManager<ChatApplicationUser> _userManager;

        public HomeController(ILogger<HomeController> logger,
            UserManager<ChatApplicationUser> userManager,
            ChatApplicationContext context)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            // Current User
            var currentUser = await _userManager.GetUserAsync(User);

            // All Users
            var userData = _userManager.Users.Where(user => true);

            // All Messages
            var messageData = _context.Message.Where(msg => true);

            foreach (var data in userData)
            {
                if (currentUser.ToString().ToLower() == data.ToString().ToLower())
                {
                    ViewBag.Email = data.UserName.ToString();
                    ViewBag.Firstname = data.FirstName.ToString();  // Check This
                    ViewBag.Lastname = data.LastName.ToString(); // Check This
                    ViewBag.Name = ViewBag.Firstname + ViewBag.Lastname; // Check This
                    break;
                }
            }

            ViewBag.AllUsers = userData;
            ViewBag.Messages = messageData;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(Message message)
        {
            //inserting msg
            message.UserName = User.Identity.Name;
            var sender = await _userManager.GetUserAsync(User);
            message.UserID = sender.Id;
            await _context.Message.AddAsync(message);
            await _context.SaveChangesAsync();

            // Current User
            var currentUser = await _userManager.GetUserAsync(User);
            //var messages = await _context.Message.ToListAsync();

            // All Users
            var userData = _userManager.Users.Where(user => true);

            // All Messages
            var messageData = _context.Message.Where(msg => true);

            foreach (var data in userData)
            {
                if (currentUser.ToString().ToLower() == data.ToString().ToLower())
                {
                    ViewBag.Email = data.UserName.ToString();
                    ViewBag.Firstname = data.FirstName.ToString();  // Check This
                    ViewBag.Lastname = data.LastName.ToString(); // Check This
                    ViewBag.Name = ViewBag.Firstname + ViewBag.Lastname; // Check This
                    break;
                }
            }

            ViewBag.AllUsers = userData;
            ViewBag.Messages = messageData;
            return View();
        }

        public void Create(string reid, string msg, string uname)
        {
            Console.WriteLine(reid + " " + msg + " " + uname);

            //string sender = _userManager.GetUserId(uname);
            Message message = new Message(uname, msg, reid);

            //await _context.Message.AddAsync(message);
            //await _context.SaveChangesAsync();

            _context.Message.Add(message);
            _context.SaveChanges();

            //return new EmptyResult();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
