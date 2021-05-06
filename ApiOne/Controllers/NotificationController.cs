using ApiOne.Hubs;
using ApiOne.Interfaces;
using ApiOne.Models.Ads;
using ApiOne.Models.Queries;
using ApiOne.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPost]
        [Route("/category/subscribe/{CatId}")]
        public IActionResult SubscribeToSubCategory([FromRoute] int CatId)
        {
            int custId = 3;
            if (CatId < 0)
            {
                return BadRequest(new { error = "something went wrong" });
            }
            var subResult = _adRepository.SubscribeToSubCategory(CatId, custId);
            if (subResult)
            {
                return Json(new { status = $"success" });
            }
            return BadRequest();
        }

        [HttpDelete]
        [Route("/category/subscribe")]
        public IActionResult RemoveFromSubscribedSubCategories([FromBody] DeleteFromCategorySub CatIds)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { error = "wrong request." });
            }
            int custId = 3;
            var deleteResult = _adRepository.RemoveFromSubscribedSubCategories(custId,CatIds.CatIds);
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
            int custId = 3;
            if (AdId < 0)
            {
                return BadRequest(new { error = "something went wrong" });
            }
            var subResult = _adRepository.AddToWishList(AdId, custId);
            if (subResult)
            {
                return Json(new { status = $"success" });
            }
            return BadRequest(new { error = "wrong adId" });
        }
        
        [HttpDelete]
        [Route("/wishlist")]
        public IActionResult DeleteFromWishList([FromBody] DeleteAdsFromWishList ids)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { error = "wrong request." });
            }
            int custId = 3;
            var deleteResult = _adRepository.RemoveFromWishList(custId, ids.AdIds);
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

        [HttpPut]
        [Route("/wishlist/notification/seen/{nId}")]
        [Produces("application/json")]
        public IActionResult NotificationSeen(int nId)
        {
            if (_adRepository.NotificationSeen(nId))
            {
                return Json(new { success=$"notificaiton:{nId} clicked!"});
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
