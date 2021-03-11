using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace identityServerNew
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            
            using(var scope = host.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

                for(int i=0; i< 10; i++)
                {
                    var user2 = new IdentityUser($"bob{i}");
                    //google .GetAwaiter().GetResult()
                    userManager.CreateAsync(user2, "password").GetAwaiter().GetResult();

                    userManager.AddClaimAsync(user2, new Claim("username", $"bob{i}")).GetAwaiter().GetResult();
                    userManager.AddClaimAsync(user2, new Claim("sta8ero", "sekatouraw123")).GetAwaiter().GetResult();
                    userManager.AddClaimAsync(user2, new Claim(ClaimTypes.Email, "bobobo@gmail.com")).GetAwaiter().GetResult();
                    userManager.AddClaimAsync(user2, new Claim("kinito", "kinito123123")).GetAwaiter().GetResult();
                    userManager.AddClaimAsync(user2, new Claim(ClaimTypes.DateOfBirth, "lalala")).GetAwaiter().GetResult();
                    userManager.AddClaimAsync(user2, new Claim(ClaimTypes.Role, "Admin")).GetAwaiter().GetResult();
                }

                for (int i = 10; i < 20; i++)
                {
                    var user = new IdentityUser
                    {
                        UserName = $"bob{i}",
                        Email = "bobobo@gmail.com",
                    };
                    //var user = new IdentityUser("bob");
                    //google .GetAwaiter().GetResult()
                    userManager.CreateAsync(user, "password").GetAwaiter().GetResult();

                    userManager.AddClaimAsync(user, new Claim("username", $"bob{i}")).GetAwaiter().GetResult();
                    userManager.AddClaimAsync(user, new Claim("sta8ero", "sekatouraw123")).GetAwaiter().GetResult();
                    //userManager.AddClaimAsync(user, new Claim(ClaimTypes.Email, "bobobo@gmail.com")).GetAwaiter().GetResult();
                    userManager.AddClaimAsync(user, new Claim("kinito", "kinito123123")).GetAwaiter().GetResult();
                    userManager.AddClaimAsync(user, new Claim(ClaimTypes.DateOfBirth, "lalala")).GetAwaiter().GetResult();
                    userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, "customer")).GetAwaiter().GetResult();
                }

            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
