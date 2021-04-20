using ApiOne.Hubs;
using ApiOne.Interfaces;
using ApiOne.Models.Ads;
using ApiOne.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Controllers
{
    public class NotificationController :Controller
    {


        private readonly IAdRepository _adRepository = new AdRepository();
        private readonly IHubContext<NotificationHub> _notificationHub;

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

        [HttpGet]
        [Route("/category/subscribe")]
        public IActionResult GetSubscribedSubCategories()
        {
            int custId = 3;
            var categories = _adRepository.GetSuscribedSubCategories(custId);
            if (categories != null)
            {
                return Json(new { categories });
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("/category/notification")]
        public IActionResult GetSubCategoryNotifications()
        {
            int custId = 3;
            var categoryNotifications = _adRepository.GetCategoryNotifications(custId);
            if (categoryNotifications != null)
            {
                return Json(categoryNotifications);
            }
            return BadRequest();
        }

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

        [HttpGet]
        [Route("/wishlist/notification")]
        public IActionResult GetWishListNotifications()
        {
            int custId = 3;
            var wishListNotifications = _adRepository.GetWishListNotifications(custId);
            if (wishListNotifications != null)
            {
                return Json(wishListNotifications);
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("/wishlist")]
        [Produces("application/json")]
        public IActionResult GetWishList()
        {
            var userId = 3;
            var updateResult = _adRepository.GetWishList(userId);
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
    }
}
