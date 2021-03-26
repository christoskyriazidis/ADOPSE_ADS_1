using IdentityServer4.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace identityServerNew.Controllers
{
    public class AdminController : Controller
    {

        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _config;

        public AdminController(
          SignInManager<IdentityUser> signInManager,
          UserManager<IdentityUser> userManager,
          IConfiguration config
          )
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _config = config;
        }

        [Authorize(Policy = "Admin")]
        [HttpGet]
        [Route("/admin/updateClaims")]
        public async Task<IActionResult> ChangeClaims()
        {
            var user = await _userManager.FindByNameAsync("bob11");

            var remove = await _userManager.RemoveClaimAsync(user, new Claim(ClaimTypes.Role, "Customer"));

            var add = await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, "Admin"));


            return Ok();
        }



        [Authorize(Policy = "Admin")]
        [HttpGet]
        [Route("/admin/customers")]
        public async Task<IActionResult> GetCustomers()
        {
            var users = await _userManager.GetUsersForClaimAsync(new Claim(ClaimTypes.Role, "Customer"));
            List<Object> userObjects = new List<object>();
            foreach (var i in users)
            {
                userObjects.Add(new
                {
                    id = i.Id,
                    username = i.UserName,
                    email = i.Email,
                    emailConfirmed = i.EmailConfirmed,
                    claims = await _userManager.GetClaimsAsync(i)
                });
            }
            return Json(userObjects);
        }


        [Authorize(Policy = "Admin")]
        [HttpGet]
        [Route("/admin/admins")]
        public async Task<IActionResult> GetAdmins()
        {

            var user = await _userManager.FindByNameAsync("bob");
            var users = await _userManager.GetUsersForClaimAsync(new Claim(ClaimTypes.Role, "Admin"));
            List<Object> userObjects = new List<object>();
            foreach (var i in users)
            {
                userObjects.Add(new
                {
                    id = i.Id,
                    username = i.UserName,
                    email = i.Email,
                    emailConfirmed = i.EmailConfirmed,
                    claims = await _userManager.GetClaimsAsync(i)
                });
            }
            return Json(userObjects);
        }



    }
}
