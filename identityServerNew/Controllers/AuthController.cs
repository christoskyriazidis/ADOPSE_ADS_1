using IdentityServer4.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Extensions.Configuration;

namespace identityServerNew.Controllers
{
    public class AuthController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IIdentityServerInteractionService _interactionService;
        private readonly IConfiguration _config;

        public AuthController(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IIdentityServerInteractionService interactionService,
            IConfiguration config
            )
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _interactionService = interactionService;
            _config = config;
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


        [Route("/mail")]
        [HttpGet]
        public async Task<IActionResult> testingMail()
        {
            var user = await _userManager.FindByNameAsync("admin");
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = HttpUtility.UrlEncode(token);
            var link = $"https://localhost:44305/verifyEmail?userId={user.Id}&token={encodedToken}";
            var mailMessage = new MailMessage
            {
                From = new MailAddress("mailservice.adopse@gmail.com"),
                Subject = "subject",
                Body = $"<h1>Welcome to our App!!!</h1><h3>Press this link to verify Your email</h3><a href=\"{link}\">Verify Email</a>",
                IsBodyHtml = true,
                To = { "christosgalaxiz@gmail.com"}
            };

            SendMail(mailMessage);
            return Ok();
        }

        [Route("/verifyEmail")]
        [HttpGet]
        public async Task<IActionResult> VerifyEmail(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return BadRequest();
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return View("ConfirmEmail", ViewBag.Message = "Verified completed!!!");
            }
            return View("ConfirmEmail", ViewBag.Message = "Verified Failed!!!");
        }


        public void SendMail(MailMessage mailMessage)
        {
            var port = _config.GetValue<int>("Email:Port");
            var appSmtpClient = _config.GetValue<string>("Email:SmtpClient");
            var appMail = _config.GetValue<string>("Email:mail");
            var password = _config.GetValue<string>("Email:password");
            var smtpClient = new SmtpClient(appSmtpClient)
            {
                Port = port,
                Credentials = new NetworkCredential(appMail, password),
                EnableSsl = true,
            };
            smtpClient.Send(mailMessage);
        }

        [Route("/forgetPassword")]
        [HttpGet]
        public async Task<IActionResult> PassowrdForget()
        {

            var user = await _userManager.FindByNameAsync("admin");
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = HttpUtility.UrlEncode(token);
            var link = $"https://localhost:44305/verifyEmail?userId={user.Id}&token={encodedToken}";
            var mailMessage = new MailMessage
            {
                From = new MailAddress("mailservice.adopse@gmail.com"),
                Subject = "Reset password Manager.!",
                Body = $"<h1>Welcome to Password Manager</h1> " +
                       $"<h3>Follow this link To change your password</h3>"+
                       $"<a href=\"{link}\">change password here!!!</a>",
                IsBodyHtml = true,
                To = { "christosgalaxiz@gmail.com" }
            };

            SendMail(mailMessage);

            return Ok();
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
            //var emailConfirmation = await _userManager.IsEmailConfirmedAsync(user);
            //if (!emailConfirmation)
            //{
            //    ViewBag.Message = "Email not verified";
            //    return View(lvm);
            //}
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
                var claims = new List<Claim>();
                claims.Add(new Claim("username", rgv.Username));
                claims.Add(new Claim("Mobile", rgv.MobilePhone));
                claims.Add(new Claim("name", rgv.Name));
                claims.Add(new Claim("lastName", rgv.LastName));
                claims.Add(new Claim(ClaimTypes.Role, "Customer"));
                claims.Add(new Claim(ClaimTypes.StreetAddress, rgv.StreetAddress));

                await _userManager.AddClaimsAsync(user, claims);
                //kanoume kai signIn (give token)
                await _signInManager.SignInAsync(user, false);
                //pane pisw apo ekei pou ir8es dhladh..
                return Redirect(rgv.ReturnUrl);
            }
            
            //else if (result.IsLockedOut)
            //{
            //    //kane kati gia to lockedOut
            //}
            return View();
        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }

        [Route("/Auth/UserAccessDenied")]
        [HttpGet]
        public ActionResult UserAccessDenied()
        {
            return View();
        }
    }
}
