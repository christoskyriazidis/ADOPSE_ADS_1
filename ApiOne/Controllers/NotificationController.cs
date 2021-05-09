using ApiOne.Hubs;
using ApiOne.Interfaces;
using ApiOne.Models.Ads;
using ApiOne.Models.Notification;
using ApiOne.Models.Queries;
using ApiOne.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Controllers
{
    public class NotificationController :Controller
    {


        private readonly IAdRepository _adRepository = new AdRepository();
        private readonly IHubContext<NotificationHub> _notificationHub;
        private readonly ICustomerRepository _customerRepo = new CustomerRepository();

        public NotificationController(IHubContext<NotificationHub> hubContext)
        {
            _notificationHub = hubContext;
        }

        [Authorize]
        [HttpPost]
        [Route("/category/subscribe/{CatId}")]
        public IActionResult SubscribeToSubCategory([FromRoute] int CatId)
        {
            var claims = User.Claims.ToList();
            var subId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var intId = _customerRepo.GetCustomerIdFromSub(subId);
            if (intId < 0)
            {
                return BadRequest(new { error = "something went wrong" });
            }
            var subResult = _adRepository.SubscribeToSubCategory(CatId, intId);
            if (subResult)
            {
                return Json(new { status = $"success" });
            }
            return BadRequest();
        }

        [Authorize]
        [HttpDelete]
        [Route("/category/subscribe")]
        public IActionResult RemoveFromSubscribedSubCategories([FromBody] DeleteFromCategorySub CatIds)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { error = "wrong request." });
            }
            var claims = User.Claims.ToList();
            var subId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var intId = _customerRepo.GetCustomerIdFromSub(subId);
            var deleteResult = _adRepository.RemoveFromSubscribedSubCategories(intId, CatIds.CatIds);
            if (deleteResult)
            {
                return Json(new { status = $"removed sub from categories!",CatIds=CatIds.CatIds });
            }
            return BadRequest(new { status = $" fail to remove sub from categories!" });
        }

        [Authorize]
        [HttpGet]
        [Route("/subcategory/subscribe")]
        public IActionResult GetSubscribedSubCategories()
        {
            var claims = User.Claims.ToList();
            var subId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var intId = _customerRepo.GetCustomerIdFromSub(subId);
            var categories = _adRepository.GetSuscribedSubCategories(intId);
            if (categories != null)
            {
                return Json(new { categories });
            }
            return BadRequest();
        }

        [Authorize]
        [HttpPost]
        [Route("/wishlist/{AdId}")]
        public IActionResult AddToWishList([FromRoute] int AdId)
        {
            var claims = User.Claims.ToList();
            var subId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var intId = _customerRepo.GetCustomerIdFromSub(subId);
            if (AdId < 0)
            {
                return BadRequest(new { error = "something went wrong" });
            }
            var subResult = _adRepository.AddToWishList(AdId, intId);
            if (subResult)
            {
                return Json(new { status = $"success" });
            }
            return BadRequest(new { error = "wrong adId" });
        }
        
        [HttpPost]
        [Route("/wishlist/remove")]
        public IActionResult DeleteFromWishList([FromBody] DeleteAdsFromWishList ids)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { error = "wrong request." });
            }
            var claims = User.Claims.ToList();
            var subId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var intId = _customerRepo.GetCustomerIdFromSub(subId);
            var deleteResult = _adRepository.RemoveFromWishList(intId, ids.AdIds);
            if (deleteResult)
            {
                return Json(new { status = "removed from wishlist!",Adids=ids.AdIds });
            }
            return BadRequest(new { status = $" fail to remove from wishlist!" });
        }

        
        [Authorize]
        [HttpGet]
        [Route("/wishlist")]
        [Produces("application/json")]
        public IActionResult GetWishList()
        {
            var claims = User.Claims.ToList();
            var subId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var intId = _customerRepo.GetCustomerIdFromSub(subId);
            var updateResult = _adRepository.GetWishList(intId);
            if (updateResult != null)
            {
                return Json(updateResult);
            }
            return BadRequest(new { error = "kati pige la8os me to wishlist(id mallon)" });
        }

        //[Authorize]
        [HttpPut]
        [Route("/notification")]
        [Produces("application/json")]
        public IActionResult NotificationSeen([FromBody]NotificationSeen notificationSeen)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return BadRequest(allErrors);
            }
            notificationSeen.Type = notificationSeen.Type.ToLower();
            if (_adRepository.NotificationSeen(notificationSeen))
            {
                return Json(new { success=$"notificaiton:{notificationSeen.Id} clicked!"});
            }
            return BadRequest(new { error = "kati pige la8os me to notification click (wishlist)" });
        }

        [Authorize]
        [HttpGet]
        [Route("/notification/{pageNumber}")]
        public IActionResult GetNotifications(int PageNumber)
        {
            var claims = User.Claims.ToList();
            var subId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var intId = _customerRepo.GetCustomerIdFromSub(subId);
            var wishListNotifications = _adRepository.GetNotifications(PageNumber, intId);
            if (wishListNotifications != null)
            {
                return Json(wishListNotifications);
            }
            return BadRequest();
        }

    }
}
