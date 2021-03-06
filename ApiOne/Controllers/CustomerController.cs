using ApiOne.Helpers;
using ApiOne.Hubs;
using ApiOne.Interfaces;
using ApiOne.Models.Ads;
using ApiOne.Models.Ads.sell;
using ApiOne.Models.Customer;
using ApiOne.Models.Mail;
using ApiOne.Models.Queries;
using ApiOne.Models.Review;
using ApiOne.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.SignalR;
using Nest;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Controllers
{
    public class CustomerController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly ICustomerRepository _customerRepo = new CustomerRepository();
        private readonly IAdRepository _adRepository = new AdRepository();
        private readonly IHubContext<NotificationHub> _notificationHub;

        public CustomerController(IWebHostEnvironment webHostEnvironment, IHubContext<NotificationHub> hubContext)
        {
            _env = webHostEnvironment;
            _notificationHub = hubContext;
        }

        [HttpGet]
        [Route("/customer")]
        public IActionResult GetCustomers([FromQuery] Pagination pagination)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { error = "Ads Out of range" });
            }
            var result = _customerRepo.GetCustomers(pagination);
            if (result == null)
            {
                return BadRequest(new { error = "customer Out of range" });
            }
            return Json(result);
        }
        
        [HttpGet]
        [Route("/customer/{id}")]
        public IActionResult GetCustomer(int id)
        {
            var result = _customerRepo.GetCustomer(id);
            if (result == null)
            {
                return BadRequest(new { error = "wrong customer id" });
            }
            return Json(result);
        }

        [Authorize]
        [HttpGet]
        [Route("/reviewNotification")]
        public IActionResult GetReviewNotifications()
        {
            var claims = User.Claims.ToList();
            var subId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var intId = _customerRepo.GetCustomerIdFromSub(subId);
            var requestedReviews = _adRepository.GetReviewNotifications(intId);
            if (requestedReviews != null)
            {
                return Json(requestedReviews);
            }
            return BadRequest(new { });
        }
        [Authorize]
        [HttpPost]
        [Route("/review")]
        public IActionResult PostReview([FromBody]PostReview postReview)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return BadRequest(allErrors);
            }
            var claims = User.Claims.ToList();
            var subId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var intId = _customerRepo.GetCustomerIdFromSub(subId);
            postReview.BuyerId = intId;
            if (_customerRepo.ReviewAndRateCustomer(postReview))
            {
                return Json(new { response="Review Submitted!"});
            }
            return BadRequest(new { message = "kati pige la8os me to review" });
        }

        [Authorize]
        [HttpGet]
        [Route("/profile")]
        public IActionResult GetProfile()
        {
            var claims = User.Claims.ToList();
            var subId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var intId = _customerRepo.GetCustomerIdFromSub(subId);
            return Json(_customerRepo.GetMyProfileInfo(intId));
        }

        [Authorize]
        [HttpGet]
        [Route("/profile/myads")]
        public IActionResult GetMyAds(Pagination adParameters)
        {
            var claims = User.Claims.ToList();
            var subId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var intId = _customerRepo.GetCustomerIdFromSub(subId); 
            return Json(_adRepository.GetMyAds(adParameters, intId));
        }
        
        [HttpPut]
        [Consumes("application/json")]
        [Route("/profile")]
        public IActionResult UpdateProfile([FromBody] CustomerDetails customerDetails)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //id
            if (_customerRepo.UpdateProfile(customerDetails)) return Json(new {message="Profile Updated Succesfully" });
            else return BadRequest(new { message = "Something went wrong (profile update)" });
        }

        [HttpPut]
        [Route("/profile/image")]
        public IActionResult UpdateProfileImage([FromForm] IFormFile img)
        {
            if(img == null)
            {
                return BadRequest(new { error = "Image cannot be null(den exeis dialeksei)" });
            }
            else if (img.ContentType != "image/png" && img.ContentType != "image/jpeg" && img.ContentType != "image/jpg")
            {
                return BadRequest(new { error = "Wrong file type (png/jpeg/jpg)" });
            }
            else if (img.Length > 3145728)
            {
                return BadRequest(new { error = "File is too big (max 3mb)" });
            }
      
           
            var claims = User.Claims.ToList();
            var subId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var intId = _customerRepo.GetCustomerIdFromSub(subId);
            if (_customerRepo.UpdateProfileImage(intId))
            {
                SingleFileUpload(img, intId);
                return Json(new { message = "profile image changed!!!" });
            }
            return BadRequest(new { error = "image change failed." });
        }

        public void SingleFileUpload(IFormFile file, int userID)
        {
            var dir = _env.ContentRootPath;
            var smallSizeAdPath = Path.Combine(dir, "Images", "profile", $"{userID}.png");
            using var image = Image.Load(file.OpenReadStream());
            image.Mutate(x => x.Resize(300, 300));
            image.Save(smallSizeAdPath);
        }

        [HttpGet]
        [Route("/secret")]
        [Authorize(Policy = "Admin")]
        public IActionResult Secret()
        {
            var claims = User.Claims.ToList();
            var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.DateOfBirth)?.Value;
            var role = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            var id = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var username = claims.FirstOrDefault(c => c.Type == "username")?.Value;
            var usernamee = claims.FirstOrDefault(c => c.Type == "usernameeee")?.Value;
            return Json(new { secret = "very secret" });
        }

        [Authorize]
        [HttpPost]
        [Route("/customer/mail")]
        public IActionResult SendMailToCustomer([FromBody] CustomerMailMessage customerMail)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return BadRequest(allErrors);
            }
            var claims = User.Claims.ToList();
            var subId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var intId = _customerRepo.GetCustomerIdFromSub(subId);
            var sender = _customerRepo.GetCustomer(intId);
            var receiver = _customerRepo.GetCustomer(customerMail.SellerId);

            var dir = _env.ContentRootPath;
            var emailTemplatePath = Path.Combine(dir, "EmailTemplates", "CustomerEmailTemplate.html");
            string template = System.IO.File.ReadAllText(emailTemplatePath);
            string template1 = template.Replace("#message#", customerMail.Message);
            string template2 = template1.Replace("#user#", sender.Username);
            string template3 = template2.Replace("#email#", sender.Email);
            string template4 = template3.Replace("#adId#", customerMail.AdId.ToString());
            string template5 = template4.Replace("#custid#", $"https://localhost:44366/home/profile/index.html?id={sender.Id}");

            var mailMessage = new MailMessage
            {
                From = new MailAddress("mailservice.adopse@gmail.com"),
                Subject = $"Customer:{sender.Username} sent you a message for your product...!!!",
                Body = template5,
                IsBodyHtml = true,
                To = { receiver.Email}
            };
            //using static class EmailService to send mail async!
            EmailService.SendMail(mailMessage);
            
            return BadRequest(new { error = "problem with email service" });
        }

        [Authorize]
        [HttpPost]
        [Route("/ad/sell")]
        public async Task<IActionResult> SellAd([FromBody]SellAdModel sellAdModel )
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return BadRequest(allErrors);
            }
            var claims = User.Claims.ToList();
            var subId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (_customerRepo.SellAd(sellAdModel.AdId,sellAdModel.BuyerId))
            {
                await _notificationHub.Clients.All.SendAsync("ReceiveWishListNotification", subId);
                await _notificationHub.Clients.All.SendAsync("ReceiveReviewNotificationsWpf", sellAdModel.AdId);
                return Json(new {response=$"Ad:{sellAdModel.AdId} sold to: {sellAdModel.BuyerId} " });
            }
            return BadRequest(new { error="Kati pige la8os me to sold ad" });
        }

        [Authorize]
        [HttpGet]
        [Route("/profile/soldAds")]
        public IActionResult GetSoldAds(Pagination pagination)
        {
            var claims = User.Claims.ToList();
            var subId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var intId = _customerRepo.GetCustomerIdFromSub(subId);
            var soldAds  = _adRepository.GetSoldAds(pagination, intId);
            if (soldAds != null)
            {
                return Json(soldAds);
            }
            return BadRequest(new { message = "You have not any bought ads.!" });
        }

        [Authorize]
        [HttpGet]
        [Route("/profile/boughtAds")]
        public IActionResult GetBoughtAds(Pagination pagination)
        {
            var claims = User.Claims.ToList();
            var subId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var intId = _customerRepo.GetCustomerIdFromSub(subId);
            var boughAds = _adRepository.GetBoughtAds(pagination, intId);
            if (boughAds != null)
            {
                return Json(boughAds);
            }
            return BadRequest(new { message ="You have not any sold ads.!" });
        }

        //[HttpGet]
        //[Route("/emaill")]
        //public async Task<IActionResult> testmail()
        //{
        //    var dir = _env.ContentRootPath;
        //    var emailTemplatePath = Path.Combine(dir, "EmailTemplates", "CustomerEmailTemplate.html");
        //    string template = System.IO.File.ReadAllText(emailTemplatePath);

        //    var mailMessage = new MailMessage
        //    {
        //        From = new MailAddress("mailservice.adopse@gmail.com"),
        //        Subject = $"Customer: vlakas sent you a message for your product...!!!",
        //        Body = template,
        //        IsBodyHtml = true,
        //        To = { "christosgalaxiz@gmail.com" }
        //    };
        //    //using static class EmailService to send mail async!
        //    EmailService.SendMail(mailMessage);
        //    return Ok();
        //}

        [HttpGet]
        [Route("/customer/review/{Cid}")]
        public IActionResult GetCustomerReviews(int Cid)
        {
            var reviews = _customerRepo.GetCustomerReviews(Cid);
            if (reviews.Any())
            {
                return Json(reviews);
            }
            return Json(new { message=$"Customer:{Cid} has no reviews yet!"});
        }

        [Authorize]
        [HttpGet]
        [Route("/loginLogs")]
        public IActionResult GetLoginLogs()
        {
            var claims = User.Claims.ToList();
            var subId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var intId = _customerRepo.GetCustomerIdFromSub(subId);
            var logs = _customerRepo.GetLoginLogs(intId);
            if (logs != null)
            {
                return Json(logs);
            }
            return Json(new {response="No loginLogs" });
        }

        [Authorize]
        [HttpPost]
        [Route("/report")]
        public IActionResult ReportAd([FromBody] ReportAd reportAd)
        {
            var claims = User.Claims.ToList();
            var subId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var intId = _customerRepo.GetCustomerIdFromSub(subId);
            if (_customerRepo.ReportAd(reportAd, intId))
            {
                return Json(new { response=$" Ad:{reportAd.AdId} Reported!"});
            }
            return Json(new { response="Exeis eidh kanei report auto to ad"});
        }

        [HttpGet]
        [Authorize]
        [Route("/test")]
        public IActionResult test()
        {
            var claims = User.Claims.ToList();
            var subId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            return Json(_customerRepo.GetCoordsBySubId(subId));
        }
    }
}
