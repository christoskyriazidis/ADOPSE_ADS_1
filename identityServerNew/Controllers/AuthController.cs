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
using identityServerNew.Model;
using identityServerNew.Helpers;

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


        //[HttpGet]
        //[Route("/sql")]
        //public IActionResult testingg()
        //{

        //    return Json(new { result= SqlServerHelpers.InsertIntoDb("takhs", "mail", "name", "lastname", "adress", "idd","phone?") }) ;
        //}
        //[Route("/cssMail")]
        //[HttpGet]
        //public async Task<IActionResult> mailSending()
        //{
        //    string texttt = System.IO.File.ReadAllText(@"C:\Users\TestFolder\index.html");
        //    var mailMessage = new MailMessage
        //    {
        //        From = new MailAddress("mailservice.adopse@gmail.com"),
        //        Subject = "subject",
        //        Body = texttt.Replace("#link#","eisaivlakaskaienazwo"),
        //        IsBodyHtml = true,
        //        To = { "christosgalaxiz@gmail.com" }
        //    };
        //    var emailStatus = await MyEmailService.SendMail(mailMessage);
        //    if (emailStatus)
        //    {
        //        return Ok();
        //    }
        //    return Json(new { error="problem with email service"});
        //}

        //apo edw stelnoum email to verification token.
        public async Task<IActionResult> EmailConfirmation(string username,string email)
        {
            var user = await _userManager.FindByNameAsync(username);
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = HttpUtility.UrlEncode(token);
            var link = $"https://localhost:44305/verifyEmail?userId={user.Id}&token={encodedToken}";
            var mailMessage = new MailMessage
            {
                From = new MailAddress("mailservice.adopse@gmail.com"),
                Subject = "subject",
                Body = $"<h1>Welcome to our App!!!</h1><h3>Press this link to verify Your email</h3><a href=\"{link}\">Verify Email</a>",
                IsBodyHtml = true,
                To = { email}
            };
            MyEmailService.SendMail(mailMessage);
           
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
                return View("ConfirmPage", ViewBag.Message = "Verified completed!!!");
            }
            return View("ConfirmPage", ViewBag.Message = "Verified Failed!!!");
        }


        [Authorize]
        [HttpPost]
        [Route("/changePassword")]
        public async Task<IActionResult> ChangePassword(ChangePassword changePassword)
        {
            if (!ModelState.IsValid)
            {
                return View(changePassword);
            }
            var claims = User.Claims.ToList();
            var id = claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            var user = await _userManager.FindByIdAsync(id);

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, changePassword.OldPassword, changePassword.NewPassword);
            if (changePasswordResult.Succeeded)
            {
                return View("ConfirmPage", ViewBag.Message = "Password change complete log in with your new password!!! ");
            }
            return View("ConfirmPage", ViewBag.Message = "Password change Failed!!! (token expired)");
        }

        [Authorize]
        [HttpGet]
        [Route("/changePassword")]
        public  IActionResult ChangePassword()
        {
            return View();
        }

        public async Task<IActionResult> SendPasswordResetToMail(string username,string email)
        {

            var user = await _userManager.FindByNameAsync(username);
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = HttpUtility.UrlEncode(token);
            var link = $"https://localhost:44305/Auth/ResetPassword?userId={user.Id}&token={encodedToken}";
            var mailMessage = new MailMessage
            {
                From = new MailAddress("mailservice.adopse@gmail.com"),
                Subject = "Reset password Manager.!",
                Body = $"<h1>Welcome to Password Manager</h1> " +
                       $"<h3>Follow this link To change your password</h3>"+
                       $"<a href=\"{link}\">change password here!!!</a>",
                IsBodyHtml = true,
                To = { email }
            };
            //send mail
            MyEmailService.SendMail(mailMessage);
            return Ok();
        }

       
        [HttpGet]
        public IActionResult ResetPasswordInput()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPasswordInput(AccountResetPassword accountResetPassword)
        {
            if (!ModelState.IsValid)
            {
                return View(accountResetPassword);
            }
            var user = await _userManager.FindByNameAsync(accountResetPassword.UsernameEmail);
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(accountResetPassword.UsernameEmail);
                if (user == null)
                {
                    ViewBag.Message = "Username Email not found.!";
                    return View(accountResetPassword);
                }
            }
            ViewBag.Message = "Verification code Send to Your email!";
            await SendPasswordResetToMail(user.UserName, user.Email);
            return View(accountResetPassword);
        }

        //view page gia resetpassword me token
        [HttpGet]
        public IActionResult ResetPassword(string token,string userId)
        {
            ResetPassword model = new ResetPassword();
            model.token = token;
            model.userId = userId;
            return View(model);
        }
        //edw ginete submit form gia  password reset me token
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPassword resetPasswrod)
        {
            if (!ModelState.IsValid)
            {
                return View(resetPasswrod);
            }
            var user = await _userManager.FindByIdAsync(resetPasswrod.userId);
            if (user == null) return BadRequest();
            var resetResult = await _userManager.ResetPasswordAsync(user, resetPasswrod.token, resetPasswrod.NewPassword);
            if (resetResult.Succeeded)
            {
                return View("ConfirmPage", ViewBag.Message = "Password reset complete log in with your new password!!!");

            }
            return View("ConfirmPage", ViewBag.Message = "Password reset Failed!!! (token expired)");

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
        public async Task<IActionResult> Login(string returnUrl)
        {
            var externalProvidres =await _signInManager.GetExternalAuthenticationSchemesAsync();
            //vazoume kai tous external sto get
            return View(new LoginViewModel { ReturnUrl = returnUrl ,ExternalProviders=externalProvidres});
        }

        public async Task<IActionResult> ExternalLogin(string provider, string returnUrl)
        {
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Auth",new { returnUrl });

            var properties =  _signInManager.ConfigureExternalAuthenticationProperties(provider,redirectUrl);
            return Challenge(properties,provider);
        }

        public async Task<IActionResult> ExternalLoginCallback(string returnUrl)
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction("Login");
            }
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider,info.ProviderKey,false);
            if (result.Succeeded)
            {
                return Redirect(returnUrl);
            }
            var username = info.Principal.FindFirst(ClaimTypes.Name).Value.Replace(" ", "_");
            return View("ExternalRegister",new ExternalRegisterViewModel {Username= username });
        }

        public async Task<IActionResult> ExternalRegister(ExternalRegisterViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            //check if username exists and throw error
            var checkUsername = await _userManager.FindByNameAsync(vm.Username);
            if (checkUsername!=null)
            {
                ViewBag.Message="Username already Exists";
                return View(vm);
            }
            //check for errors
            var info = await _signInManager.GetExternalLoginInfoAsync();
            var stats = info.Principal.Claims.ToList();
            if (info == null)
            {
                return RedirectToAction("Login");
            }
            var externalClaims = info.Principal.Claims.ToList();
            var emailClaim = externalClaims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            //var nameClaim = externalClaims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            var user = new IdentityUser
            {
                UserName = vm.Username,
                Email = emailClaim,
            };
            var result = await _userManager.CreateAsync(user);

            if (!result.Succeeded)
            {
                return View(vm);
            }

            var claims = new List<Claim>();
            claims.Add(new Claim("username", vm.Username));
            claims.Add(new Claim(ClaimTypes.Role, "Customer"));
            var claimResult = await _userManager.AddClaimsAsync(user, claims);
            var emailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var emailConfirm = await _userManager.ConfirmEmailAsync(user, emailToken);

            //var coords = await MapsHelpers.GetCoordsFromAddress(rgv.StreetAddress, rgv.PostalCode);
            LocationModel locationModel = new LocationModel{Address="waiting for address",Latitude=-1,Longitude=-1 };
            
            var adResult = SqlServerHelpers.InsertIntoDb(vm.Username, emailClaim, info.ProviderDisplayName, info.ProviderDisplayName, info.ProviderDisplayName, user.Id, info.ProviderDisplayName, locationModel);
            if (!claimResult.Succeeded)
            {
                return View(vm);
            }
            var addLoginResult = await _userManager.AddLoginAsync(user, info);
            if (!addLoginResult.Succeeded)
            {
                return View(vm);
            }
            //perasei ola auta tote redirect
            TempData["register"] = $"Registration success {info.ProviderDisplayName}";
            return Redirect(vm.ReturnUrl);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel lvm)
        {
            var externalProvidres = await _signInManager.GetExternalAuthenticationSchemesAsync();

            if (!ModelState.IsValid)
            {
                return View(new LoginViewModel {ExternalProviders=externalProvidres,ReturnUrl=lvm.ReturnUrl,Username=lvm.Username});
            }
            //vlepoume an einai valid to request (model)
            if (lvm.Password == null || lvm.Username == null)
            {
                ViewBag.Message = "You must fill the form ";
                return View(new LoginViewModel { ExternalProviders = externalProvidres, ReturnUrl = lvm.ReturnUrl, Username = lvm.Username });
            }

            //koitaw an uparxei to email PRWTA
            var user = await _userManager.FindByEmailAsync(lvm.Username);
            if (user == null)
            {
                user = await _userManager.FindByNameAsync(lvm.Username);
                if (user == null)
                {
                    ViewBag.Message = $"Wrong username or password";
                    return View(new LoginViewModel { ExternalProviders = externalProvidres, ReturnUrl = lvm.ReturnUrl, Username = lvm.Username });
                }
            }
            //check if email is confirmed
            var emailConfirmation = await _userManager.IsEmailConfirmedAsync(user);
            if (!emailConfirmation)
            {
                ViewBag.Message = "Email not verified";
                return View(new LoginViewModel { ExternalProviders = externalProvidres, ReturnUrl = lvm.ReturnUrl, Username = lvm.Username });
            }
            var loginResult = await _signInManager.PasswordSignInAsync(user, lvm.Password, false, true);
            if (loginResult.IsLockedOut)
            {
                ViewBag.Message = $"You have been locked out. wait 1 minute. ";
                return View(new LoginViewModel { ExternalProviders = externalProvidres, ReturnUrl = lvm.ReturnUrl, Username = lvm.Username });
            }
            else if (loginResult.Succeeded)
            {
                if (user.UserName != "admin")
                {
                    //SqlServerHelpers.LoginLogs(user.Id, "10.l2.1.1");
                }
                return Redirect(lvm.ReturnUrl);
            }
            var failedTimes = await _userManager.GetAccessFailedCountAsync(user);
            ViewBag.Message = $"Wrong password. You have {4 - failedTimes} more tries";
            return View(new LoginViewModel { ExternalProviders = externalProvidres, ReturnUrl = lvm.ReturnUrl, Username = lvm.Username });
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
                ViewBag.Message = "Username already exists";
                return View(rgv);
            }
            var user = new IdentityUser
            {
                UserName = rgv.Username,
                Email = rgv.Email,
            };
            var coords = await MapsHelpers.GetCoordsFromAddress(rgv.StreetAddress,rgv.PostalCode);
            if (coords == null)
            {
                ViewBag.Message = "Wrong address/postalcode";
                return View(rgv);
            }
            var result = await _userManager.CreateAsync(user, rgv.Password);
            var adResult = SqlServerHelpers.InsertIntoDb(rgv.Username, rgv.Email, rgv.Name, rgv.LastName, rgv.StreetAddress, user.Id, rgv.MobilePhone, coords);
            var xorafiResult = SqlServerHelpers.InsertIntoDypaDb(rgv.Username, rgv.Email, rgv.Name, rgv.LastName, rgv.StreetAddress, user.Id, rgv.MobilePhone, coords);

            if (result.Succeeded && adResult && xorafiResult)
                //if (result.Succeeded)
            {
                var claims = new List<Claim>();
                claims.Add(new Claim("username", rgv.Username));
                //claims.Add(new Claim("Mobile", rgv.MobilePhone));
                claims.Add(new Claim("name", rgv.Name));
                claims.Add(new Claim("lastName", rgv.LastName));
                claims.Add(new Claim(ClaimTypes.Role, "Customer"));
                claims.Add(new Claim(ClaimTypes.StreetAddress, rgv.StreetAddress));

                await _userManager.AddClaimsAsync(user, claims);
                //stelnoume sto mail tou xrhsth to token...
                await EmailConfirmation(rgv.Username, rgv.Email);
                //pane pisw apo ekei pou ir8es dhladh..
                TempData["register"] = "Registration success please verify your email.";
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

        //view gia to access denied
        [Route("/Auth/UserAccessDenied")]
        [HttpGet]
        public ActionResult UserAccessDenied()
        {
            return View();
        }
    }
}
