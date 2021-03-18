using ApiOne.Databases;
using ApiOne.Hubs;
using ApiOne.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Controllers
{
    public class dbController : Controller
    {
        private readonly IHubContext<ChatHub> _myHub;

        public dbController(IHubContext<ChatHub> hubContext)
        {
            _myHub = hubContext;

        }
        private readonly Database database = Database.GetInstance();
        public MyErrorObject CheckAdForErrors(Product product)
        {
            foreach (PropertyInfo pi in product.GetType().GetProperties())
            {
                if (pi.PropertyType == typeof(string))
                {
                    string value = (string)pi.GetValue(product);
                    if (string.IsNullOrEmpty(value))
                    {
                        return new MyErrorObject { Error = $"Cannot be null or empty {pi}" };
                    }
                }
                else if (pi.PropertyType == typeof(int))
                {
                    int value = (int)pi.GetValue(product);
                    if (value <= 0)
                    {
                        return new MyErrorObject { Error = $"Cannot be null/empty or  <= 0 {pi}" };
                    }
                }
            }
            return new MyErrorObject { Error = null };
        }

        [HttpGet]
        [Route("/ad")]
        [Produces("application/json")]
        public JsonResult  GetAds()
        {
            return Json(database.GetAds());
        }


        [HttpGet]
        [Route("/category")]
        [Produces("application/json")]
        public JsonResult GetCategories()
        {
            return Json(database.GetCategories());
        }
        

        [HttpGet]
        [Route("/condition")]
        [Produces("application/json")]
        public JsonResult GetCondition()
        {
            return Json(database.GetCondition());
        }

        //[Authorize]
        [Route("/ad")]
        [HttpPost]
        [Consumes("application/json")]
        public IActionResult AddAd([FromBody] Product product)
        {
            MyErrorObject error = CheckAdForErrors(product);
            if (error.Error != null)
            {
                return BadRequest(error);
            }
            //database stuff add product
            return Json(product);
        }


        [HttpPut]
        [Route("/ad/{id}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> UpdateAd(int id,[FromBody] Ad ad)
        {
            if(database.UpdateAd(id, ad))
            {
                await _myHub.Clients.All.SendAsync("wishListNotification");
                return Ok();
            }
            return BadRequest();
        }

        [HttpDelete]
        [Route("/ad/{id}")]
        public IActionResult DeleteAd([FromRoute] long id)
        {
            //if error
            //else ok
            return Json(new { message = $"byebye {id}" });
        }
        
        //[Authorize]
        [HttpGet]
        [Route("/notification")]
        public IActionResult GetWishListNotification()
        {
            return Json(database.GetWishListNotification(1));
        }

        [HttpGet]
        [Route("/test")]
        public async Task<IActionResult> ttess()
        {
            var users = ChatHub.ConnectedUsers;
            foreach (string i in users)
            {
                await _myHub.Clients.Client(i).SendAsync("wishListNotification");
            }
            return Ok();
        }

    }
}
