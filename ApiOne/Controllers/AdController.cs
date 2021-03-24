using ApiOne.Databases;
using ApiOne.Hubs;
using ApiOne.Interfaces;
using ApiOne.Models;
using ApiOne.Models.Queries;
using ApiOne.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Controllers
{
    public class AdController : Controller
    {
        private readonly IAdRepository _adRepository = new AdRepository();
        private readonly IHubContext<ChatHub> _myHub;

        public AdController(IHubContext<ChatHub> hubContext)
        {
            _myHub = hubContext;
        }

        //no authorize gia na vlepoun oi episkeptes
        //valid query String (value < 1) ? 1 : value; pageSize = (value > maxPageSize||value<5) ? maxPageSize : value;
        [HttpGet]
        [Route("/ad")]
        [Produces("application/json")]
        public IActionResult GetAds([FromQuery] AdPageSizeNumberParameters adParameters)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { error = "Ads Out of range" });
            }
            var ads = _adRepository.GetAds(adParameters);
            if (ads.Ads == null||ads.Ads.Count==0||ads.TotalAds<1)
            {
                return BadRequest(new { error = "Ads Out of range" });
            }
            //Response.Headers.Add("Ad-Pagination", JsonConvert.SerializeObject(pagination));
            return Json(ads);
        }

        //no authorize gia na vlepoun oi episkeptes
        [HttpGet]
        [Route("/ad/{id}")]
        [Produces("application/json")]
        public IActionResult GetAd(int id)
        {
            var ad = _adRepository.GetAd(id);
            if (ad == null)
            {
                return BadRequest(new { error = "ad not found" });
            }
            return Json(ad);
        }

        //[Authorize]
        [Route("/ad")]
        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/json")]
        public IActionResult AddAd([FromBody] Ad ad)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return BadRequest(allErrors);
            }
            if (_adRepository.InsertAd(ad))
            {
                return Ok(ad);
            }
            return Json(new { error = "error" });
        }

        [HttpPut]
        [Route("/ad")]
        [Produces("application/json")]
        [Consumes("application/json")]
        public IActionResult UpdateAd([FromBody] Ad ad)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return BadRequest(allErrors);
            }
            var updateResult = _adRepository.UpdateAd(ad);
            if (updateResult != null)
            {
                return Json(updateResult);
            }
            return BadRequest();
        }

        [HttpDelete]
        [Route("/ad/{id}")]
        public IActionResult DeleteAd([FromRoute] int id)
        {
            if (id<0)
            {
                return BadRequest(new {error="wrong id"});
            }
            var deleteResult = _adRepository.DeleteAd(id);
            if (deleteResult)
            {
                return Json(new { status=$"{id} deleted!"});
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("/filter")]
        public IActionResult Testt([FromQuery] ParamTypesFilter paramTypeFilter,[FromQuery] AdPageSizeNumberParameters adParameters)
        {
            AdParametresQuertyFilter adParametresFilter = new AdParametresQuertyFilter(adParameters);

            foreach (var prop in paramTypeFilter.GetType().GetProperties())
            {
                var value = prop.GetValue(paramTypeFilter, null);
                if (value != null)
                {
                    String[] filterArray = value.ToString().Split("_").ToArray();
                    string sqlIn = String.Join(",", filterArray);
                    string last =$"{prop.Name} IN ({sqlIn})";
                    adParametresFilter.GetType().GetProperty(prop.Name).SetValue(adParametresFilter,last);
                }
            }

            return Json(new { msg = "hello" });
        }

        //[HttpGet]
        //[Route("/category")]
        //[Produces("application/json")]
        //public JsonResult GetCategories()
        //{
        //    return Json(database.GetCategories());
        //}
       
        
        //[HttpGet]
        //[Route("/condition")]
        //[Produces("application/json")]
        //public JsonResult GetCondition()
        //{
        //    return Json(database.GetCondition());
        //}



        ////[Authorize]
        //[HttpGet]
        //[Route("/notification")]
        //public IActionResult GetWishListNotification()
        //{
        //    return Json(database.GetWishListNotification(1));
        //}

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
