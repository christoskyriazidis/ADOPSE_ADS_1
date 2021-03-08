using IdentityServer4.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace identityServerNew.Controllers
{
    public class AuthController :Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IIdentityServerInteractionService _interactionService;

        public AuthController(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IIdentityServerInteractionService interactionService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _interactionService = interactionService;
        }

        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            await _signInManager.SignOutAsync();

            var logoutRequest = await _interactionService.GetLogoutContextAsync(logoutId);

            if (string.IsNullOrEmpty(logoutRequest.PostLogoutRedirectUri))
            {
                return RedirectToAction("Index", "Home");
            }

            return Redirect(logoutRequest.PostLogoutRedirectUri);
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel { ReturnUrl=returnUrl});
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel lvm)
        {
            if (!ModelState.IsValid)
            {
                return View(lvm);
            }
            //vlepoume an einai valid to request (model)
            if (lvm.Password==null || lvm.Username ==null)
            {
                ViewBag.Message = "You must fill the form dumb kid!";
                return View(lvm);
            }

            //var user = await _userManager.FindByEmailAsync(lvm.Username);
            //var emailtest = user.EmailConfirmed;

            var result = await _signInManager.PasswordSignInAsync(lvm.Username, lvm.Password, false, true);
            if (result.IsLockedOut)
            {
                ViewBag.Message = "You have been lockedOut wait 1 minute.";
                return View(lvm);
                //kane kati gia to lockedOut
            }
            else if (result.Succeeded)
            {
                //pane pisw apo ekei pou ir8es dhladh..
                return Redirect(lvm.ReturnUrl);
            }
            ViewBag.Message = "Wrong username or password";
            return View(lvm);
        }


        [HttpGet]
        public IActionResult Register(string returnUrl)
        {
            return View(new RegisterViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel rgv)
        {
            //google modelstate
            if (!ModelState.IsValid)
            {
                return View(rgv);
            }
            
            var userExists = await _userManager.FindByNameAsync(rgv.Username);
            if (userExists != null)
            {
                ViewBag.Message = "yparxei o user";
                return View(rgv);
            }

            var user = new IdentityUser
            {
                UserName = rgv.Username,
                Email = rgv.Email,
            };

            var result =await _userManager.CreateAsync(user, rgv.Password);

            if (result.Succeeded)
            {
                var emailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                var link = Url.Action(nameof(VerifyEmail), "Home", new { userId = user.Id, emailToken }, Request.Scheme, Request.Host.ToString());

                var claims = new List<Claim>();
                //claims.Add(new Claim("email", rgv.Email));
                //claims.Add(new Claim("kinito", rgv.MobilePhone));
                //claims.Add(new Claim("username", rgv.Name));
                //claims.Add(new Claim(ClaimTypes.StreetAddress, rgv.StreetAddress));

                //await _userManager.AddClaimsAsync(user, claims);
                //kanoume kai signIn (give token)
                //await _signInManager.SignInAsync(user, false);

                //pane pisw apo ekei pou ir8es dhladh..
                return Redirect(rgv.ReturnUrl);
            }
            
            //else if (result.IsLockedOut)
            //{
            //    //kane kati gia to lockedOut
            //}
            return View();
        }

        public async Task<IActionResult> VerifyEmail(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null) return BadRequest();

            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (result.Succeeded)
            {
                return View();
            }

            return BadRequest();
        }

        public IActionResult EmailVerification() => View();

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult UserAccessDenied()
        {
            return View();
        }
    }
}
