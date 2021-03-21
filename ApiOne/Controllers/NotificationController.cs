using ApiOne.Hubs;
using ApiOne.Interfaces;
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
        private readonly IHubContext<ChatHub> _myHub;

        public NotificationController(IHubContext<ChatHub> hubContext)
        {
            _myHub = hubContext;
        }

        [HttpPost]
        [Route("/category/subscribe/{CatId}")]
        public IActionResult SubscribeToCategory([FromRoute] int CatId)
        {
            int custId = 3;
            if (CatId < 0)
            {
                return BadRequest(new { error = "something went wrong" });
            }
            var subResult = _adRepository.SubscribeToCategory(CatId, custId);
            if (subResult)
            {
                return Json(new { status = $"success" });
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("/category/subscribe")]
        public IActionResult GetSubscribedCategories([FromRoute] int CatId)
        {
            int custId = 3;
            if (CatId < 0)
            {
                return BadRequest(new { error = "something went wrong" });
            }
            var categories = _adRepository.GetSuscribedCategories(custId);
            if (categories != null)
            {
                return Json(new { categories });
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("/category/notification")]
        public IActionResult GetCategoryNotifications()
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
            return BadRequest();
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

    }
}
