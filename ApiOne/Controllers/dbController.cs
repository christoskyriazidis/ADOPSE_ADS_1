using ApiOne.Databases;
using ApiOne.Hubs;
using ApiOne.Interfaces;
using ApiOne.Models;
using ApiOne.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Controllers
{
    public class dbController : Controller
    {
        private  readonly IAdRepository _adRepository = new AdRepository();
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

        //[Authorize]
        [HttpGet]
        [Route("/ad")]
        [Produces("application/json")]
        public JsonResult  GetAds()
        {
            return Json(_adRepository.GetAds());
        } 
        
        [HttpGet]
        [Route("/ad/{id}")]
        [Produces("application/json")]
        public IActionResult GetAd(int id)
        {
            var ad=_adRepository.GetAd(id);
            if (ad == null)
            {
                return BadRequest(new { error = "ad not found" });
            }
            return Json(ad);
        }

        //[Authorize]
        [Route("/ad")]
        [HttpPost]
        [Consumes("application/json")]
        public IActionResult AddAd([FromBody] Ad ad)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return BadRequest(allErrors);
            }
            _adRepository.InsertAd(ad);
            return Ok(ad);
        }

        [HttpPut]
        [Route("/ad")]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> UpdateAd([FromBody] Ad ad)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return BadRequest(allErrors);
            }
            var updateResult = _adRepository.UpdateAd(ad);
            if (updateResult!=null)
            {
                return Json(updateResult);
            }
            return BadRequest();
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
