using System;
using Chat_Application.Areas.Identity.Data;
using Chat_Application.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(Chat_Application.Areas.Identity.IdentityHostingStartup))]
namespace Chat_Application.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<Chat_ApplicationContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("Chat_ApplicationContextConnection")));

                services.AddDefaultIdentity<Chat_ApplicationUser>(options => 
                {
                    options.SignIn.RequireConfirmedAccount = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                })
                    .AddEntityFrameworkStores<Chat_ApplicationContext>();
            });
        }
    }
}