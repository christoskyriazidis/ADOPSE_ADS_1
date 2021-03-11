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
    public class AuthController : Controller
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

        [Authorize]
        [Route("/testt")]
        [HttpGet]
        public async Task<IActionResult> UpdateClaims()
        {
            var claims = User.Claims.ToList();
            var name = claims.FirstOrDefault(c => c.Type == "name")?.Value;
            var idClaim = claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            //var name = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            var userId = await _userManager.FindByIdAsync(idClaim);
            var user = await _userManager.FindByNameAsync(name);

            var token = await _userManager.GenerateChangeEmailTokenAsync(user, "lalal@mail.com");
            var newEmail = await _userManager.ChangeEmailAsync(user, "lalal@mail.com", token);
            var newClaim = await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.MobilePhone, "12312"));

            var tokenPassword = await _userManager.GeneratePasswordResetTokenAsync(user);
            var newPassword = await _userManager.ResetPasswordAsync(user, tokenPassword, "eimaivlakas");

            var user2 = await _userManager.FindByNameAsync("bob");
            var newClaims = User.Claims.ToList();


            return Json(new { test = "s" });
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
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel lvm)
        {
            if (!ModelState.IsValid)
            {
                return View(lvm);
            }
            //vlepoume an einai valid to request (model)
            if (lvm.Password == null || lvm.Username == null)
            {
                ViewBag.Message = "You must fill the form dumb kid!";
                return View(lvm);
            }

            //koitaw an uparxei to email PRWTA
            var user = await _userManager.FindByEmailAsync(lvm.Username);
            if (user == null)
            {
                user = await _userManager.FindByNameAsync(lvm.Username);
                if (user == null)
                {
                    ViewBag.Message = $"Wrong username or password (den uparxei o xrhsths/email gia ekpedeutikous logous oxi asfaleia)";
                    return View(lvm);
                }
            }
            var loginResult = await _signInManager.PasswordSignInAsync(user, lvm.Password, false, true);
            if (loginResult.IsLockedOut)
            {
                ViewBag.Message = $"You have been lockedOut wait 1 minute. ";
                return View(lvm);
            }
            else if (loginResult.Succeeded)
            {
                return Redirect(lvm.ReturnUrl);
            }
            var failedTimes = await _userManager.GetAccessFailedCountAsync(user);
            ViewBag.Message = $"Wrong password.Sou menoun akoma {4 - failedTimes} prospa8ies";
            return View(lvm);
        }

        [HttpGet]
        [Route("/customers")]
        public async Task<IActionResult> UserInfo()
        {
            var users = await _userManager.GetUsersForClaimAsync(new Claim(ClaimTypes.Role, "customer"));
            List<Object> userObjects = new List<object>();
            foreach(var i in users)
            {
                userObjects.Add(new {
                    id = i.Id,
                    username = i.UserName,
                    email = i.Email,
                    emailConfirmed = i.EmailConfirmed,
                    claims = await _userManager.GetClaimsAsync(i)
            });
            }

            return Json(userObjects);
        }
        [Authorize(Roles ="Admin")]
        [HttpGet]
        [Route("/admins")]
        public async Task<IActionResult> UserInfoA()
        {
            var user = await _userManager.FindByNameAsync("bob");
            var users = await _userManager.GetUsersForClaimAsync(new Claim(ClaimTypes.Role, "Admin"));

            List<Object> userObjects = new List<object>();

            foreach(var i in users)
            {
                userObjects.Add(new {
                    id = i.Id,
                    username = i.UserName,
                    email = i.Email,
                    emailConfirmed = i.EmailConfirmed,
                    claims = await _userManager.GetClaimsAsync(i)
                });
            }

            return Json(userObjects);
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
