using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Chat_Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Chat_Application.Areas.Identity.Data;

namespace Chat_Application.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        
        private readonly SignInManager<Chat_ApplicationUser> signInManager;
        private readonly UserManager<Chat_ApplicationUser> userManager;

        public HomeController(ILogger<HomeController> logger, SignInManager<Chat_ApplicationUser> _signInManager, UserManager<Chat_ApplicationUser> _userManager)
        {
            _logger = logger;
            signInManager = _signInManager;
            userManager = _userManager;
        }

        public IActionResult Index()
        {
            // ViewData
            ViewBag.UserName = userManager.GetUserName(HttpContext.User);
            return View();
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
