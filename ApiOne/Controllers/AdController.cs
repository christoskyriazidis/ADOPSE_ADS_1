using ApiOne.Databases;
using ApiOne.Hubs;
using ApiOne.Interfaces;
using ApiOne.Models;
using ApiOne.Models.Ads;
using ApiOne.Models.Queries;
using ApiOne.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.SignalR;
using Nest;
using Newtonsoft.Json;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Controllers
{
    public class AdController : Controller
    {
        private readonly IAdRepository _adRepository = new AdRepository();
        private readonly IWebHostEnvironment _env;
        private readonly IHubContext<ChatHub> _myHub;

        public AdController(IHubContext<ChatHub> hubContext, IWebHostEnvironment webHostEnvironment)
        {
            _env = webHostEnvironment;
            _myHub = hubContext;
        }

        //no authorize gia na vlepoun oi episkeptes
        //valid query String (value < 1) ? 1 : value; pageSize = (value > maxPageSize||value<5) ? maxPageSize : value;
        //[Authorize]
        [HttpGet]
        [Route("/ad")]
        [Produces("application/json")]
        public IActionResult GetAds([FromQuery] Pagination adParameters)
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
        //[Produces("application/json")]
        //[Consumes("application/json")]
        public IActionResult AddAd([FromForm] CreateAd ad)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return BadRequest(allErrors);
            }
            //vazoume to id tou xrhsth sto object Ad 
            ad.Customer = 3;
            //koitaw an einai null to img 
            if (ad.Img == null) ad.NewImg = "No";
            else ad.NewImg = "NewImg";
            int result = _adRepository.InsertAd(ad);
            switch (result)
            {
                case -2: return Json(new { error = "something went wrong with ad creation! " });
                case -1: return Json(new { success = "ad Added successfully created.! with default img!" });
                case > 0:
                    SingleFileUpload(ad.Img, result);
                    return Json(new { success = "ad Added successfully created.! with users img!" });
                default: return Json(new { error = "something went wrong with ad creation! " });
            }
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
        public IActionResult Testt([FromQuery] AdFiltersFromParam paramTypeFilter,[FromQuery] Pagination adParameters)
        {
            if (string.IsNullOrEmpty(paramTypeFilter.State) && string.IsNullOrEmpty(paramTypeFilter.Manufacturer) && string.IsNullOrEmpty(paramTypeFilter.Type) && string.IsNullOrEmpty(paramTypeFilter.Condition) && string.IsNullOrEmpty(paramTypeFilter.Category))
            {
                return BadRequest(new { error = "you should use at least one filter"});
            }
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return BadRequest(allErrors);
            }
            AdParametresQueryFilterBack adParametresQueryFilterBack = new AdParametresQueryFilterBack();
            string filterBox ="";
            foreach (var prop in paramTypeFilter.GetType().GetProperties())
            {
                var value = prop.GetValue(paramTypeFilter, null);
                if (value != null)
                {
                    String[] filterArray = value.ToString().Split("_").ToArray();
                    string sqlIn = String.Join(",", filterArray);
                    string last =$"{prop.Name} IN ({sqlIn})";
                    adParametresQueryFilterBack.GetType().GetProperty(prop.Name).SetValue(adParametresQueryFilterBack, value.ToString());
                    filterBox += $"{last} or ";
                }
            }
            //vgazoume to teleuaio or... :)
            adParametresQueryFilterBack.FinalQuery = filterBox.Remove(filterBox.Length - 3);
            var filteredAds = _adRepository.GetAdsByFilters(adParametresQueryFilterBack,adParameters);
            if (filteredAds.Ads.Count < 1)
            {
                return Json(new { result="There are still no ads with current filters",filters=paramTypeFilter });
            }
            return Json(filteredAds);
        }

        

        [HttpGet]
        [Route("/condition")]
        [Produces("application/json")]
        public IActionResult GetConditions()
        {
            var conditions = _adRepository.GetConditions();
            if (conditions != null)
            {
                return Json(conditions);
            }
            return BadRequest(new { error = "kati pige la8os me ta conditions" });
        }
        
        [HttpGet]
        [Route("/state")]
        [Produces("application/json")]
        public IActionResult GetStates()
        {
            var states = _adRepository.GetStates();
            if (states != null)
            {
                return Json(states);
            }
            return BadRequest(new { error="kati pige la8os me to wishlist"});
        }  
        
        [HttpGet]
        [Route("/type")]
        [Produces("application/json")]
        public IActionResult GetTypes()
        {
            var types = _adRepository.GetTypes();
            if (types != null)
            {
                return Json(types);
            }
            return BadRequest(new { error= "kati pige la8os me to types" });
        }

        [HttpGet]
        [Route("/category")]
        [Produces("application/json")]
        public IActionResult GetCategories()
        {
            var categories = _adRepository.GetCategories();
            if (categories != null)
            {
                return Json(categories);
            }
            return BadRequest(new { error = "kati pige la8os me ta categories" });
        }
        
        [HttpGet]
        [Route("/manufacturer")]
        [Produces("application/json")]
        public IActionResult GetManufacturers()
        {
            var manufacturers = _adRepository.GetManufacturers();
            if (manufacturers != null)
            {
                return Json(manufacturers);
            }
            return BadRequest(new { error = "kati pige la8os me ta manufacturers" });
        }

        //[Authorize]
        [HttpPut]
        [Route("/ad/image")]
        public IActionResult UpdateImage(IFormFile img,int adId)
        {

            SingleFileUpload(img, adId);
            if (_adRepository.UpdateAdImg(adId))
            {
                return Json(new { message="img successfully changed!!"});
            }
            return BadRequest(new {error="something went wrong with img update" });
        }

        public IActionResult SingleFileUpload(IFormFile file,int adId)
        {
            if (file.Length > 3145728)
            {
                return BadRequest(new { error = "File is too big (max 3mb)" });
            }
            if (file.ContentType != "image/png" && file.ContentType != "image/jpeg" && file.ContentType != "image/jpg")
            {
                return BadRequest(new { error = "Wrong file type" });
            }
            var dir = _env.ContentRootPath;
            var smallSizeAdPath = Path.Combine(dir, "Images", "serverA", "small",$"{adId}.png");
            using var image = Image.Load(file.OpenReadStream());
            image.Mutate(x => x.Resize(100, 100));
            image.Save(smallSizeAdPath);
            var FullSizeAdPath = Path.Combine(dir, "Images", "serverA", "full", $"{adId}.png");
            using (var fileStream = new FileStream(FullSizeAdPath, FileMode.Create, FileAccess.Write))
            {
                file.CopyTo(fileStream);
                
            }
            return Ok();
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

        [Route("/yes")]
        public IActionResult SearchWithFilters([FromQuery] AdFiltersFromParam paramTypeFilter, Pagination pagination)
        {
            //if (string.IsNullOrEmpty(paramTypeFilter.State) && string.IsNullOrEmpty(paramTypeFilter.Manufacturer) && string.IsNullOrEmpty(paramTypeFilter.Type) && string.IsNullOrEmpty(paramTypeFilter.Condition) && string.IsNullOrEmpty(paramTypeFilter.Category) && string.IsNullOrEmpty(paramTypeFilter.Title) && string.IsNullOrEmpty(paramTypeFilter.Description))
            //{
            //    return BadRequest(new { error = "you should use at least one filter" });
            //}
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return BadRequest(allErrors);
            }
            AdFiltersFromParamClient adFiltersFromParamClient = new AdFiltersFromParamClient(pagination);
            foreach (var prop in paramTypeFilter.GetType().GetProperties())
            {
                var value = prop.GetValue(paramTypeFilter, null);
                if (value != null && prop.Name != "Title" && prop.Name != "Description")
                {
                    var filterArray = value.ToString().Split("_");
                    var filterIntArray = filterArray.Select(Int32.Parse).ToList();
                    adFiltersFromParamClient.GetType().GetProperty(prop.Name).SetValue(adFiltersFromParamClient, filterIntArray);
                }
            }

            var settings = new ConnectionSettings(new Uri("http://localhost:9200/"))
                .DefaultIndex("ads");

            var client = new ElasticClient(settings);

            var searchRequest = new SearchRequest<CompleteAd>
            {
                Query = new MatchAllQuery(),
            };

            var searchResponse11 = client.Search<CompleteAd>(s => s
           .Size(100)
               .Query(q => q
                .Regexp(r => r
                        .Field(f => f.Title)
                        .Value($"{paramTypeFilter.Title}")
                    )
                ||
                    q.Regexp(r => r
                        .Field(f => f.Description)
                        .Value($"{paramTypeFilter.Description}")
                        )
                &&

                    q.Terms(t => t
                        .Field(f => f.Manufacturer)
                        .Terms<int>(adFiltersFromParamClient.Manufacturer)
                    )
                &&
                    q.Range(r => r.
                        Field(f => f.Price)
                        .GreaterThanOrEquals(0)
                        .LessThanOrEquals(1000)
                        )
                &&
                q.Terms(t => t
                        .Field(f => f.Type)
                        .Terms<int>(adFiltersFromParamClient.Type)
                    )
                )
          );

            return Ok(searchResponse11);
        }


    }
}
