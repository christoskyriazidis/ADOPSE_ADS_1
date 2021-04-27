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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Controllers
{
    public class AdController : Controller
    {
        private readonly IAdRepository _adRepository = new AdRepository();
        private readonly ICustomerRepository _customerRepo = new CustomerRepository();
        private readonly IWebHostEnvironment _env;
        private readonly IHubContext<NotificationHub> _NotificationHub;

        public AdController(IHubContext<NotificationHub> hubContext, IWebHostEnvironment webHostEnvironment)
        {
            _env = webHostEnvironment;
            _NotificationHub = hubContext;
        }

        //no authorize gia na vlepoun oi episkeptes
        //valid query String (value < 1) ? 1 : value; pageSize = (value > maxPageSize||value<5) ? maxPageSize : value;
        //[Authorize]
        //[HttpGet]
        //[Route("/ad")]
        //[Produces("application/json")]
        //public IActionResult GetAds([FromQuery] Pagination adParameters)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(new { error = "Ads Out of range" });
        //    }
        //    var ads = _adRepository.GetAds(adParameters);
        //    if (ads.Result == null||ads.Result.Count==0||ads.TotalAds<1)
        //    {
        //        return BadRequest(new { error = "Ads Out of range" });
        //    }
        //    //Response.Headers.Add("Ad-Pagination", JsonConvert.SerializeObject(pagination));
        //    return Json(ads);
        //}

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

        [Authorize(Policy = "Admin")]
        [Route("/denkserw")]
        public IActionResult denkserw()
        {
            var claims = User.Claims.ToList();
            var subId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var test = _customerRepo.GetCustomerIdFromSub(subId);
            return Json(new { message = "douleuei" });
        }

        [Authorize]
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
            string subId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var claims = User.Claims.ToList();

            ad.Customer = _customerRepo.GetCustomerIdFromSub("f4ebd4c3-b7f5-4df1-8750-5a437fdaa0fc");
            //koitaw an einai null to img 
            if (ad.Img == null) ad.NewImg = "No";
            else ad.NewImg = "NewImg";
            int result = _adRepository.InsertAd(ad);
            switch (result)
            {
                case -2: return BadRequest(new { error = "something went wrong with ad creation! " });
                case -1: return Json(new { success = "ad Added successfully created.! with default img!" });
                case > 0:
                    SingleFileUpload(ad.Img, result);
                    return Json(new { success = "ad Added successfully created.! with users img!" });
                default: return Json(new { error = "something went wrong with ad creation! " });
            }
        }

        [HttpGet]
        [Route("/ox")]
        public IActionResult testt()
        {
            CreateAd ad = new CreateAd();
            Random rnd = new Random();

            string[] titles = { "IPHONE 5S - SILVER - UNLOCKED in Birmingham", "iPhone XR unlocked 64gb in Coleraine", "Immaculate Samsung S10 mobile", "Samsung galaxy fe brand new unused £370", "kserw den ad", "kalimera", "kalinixta" };
            string[] descriptions = { "Wireless that goes the distance: Basement rec room? Backyard movie night? Bring ‘em on. The long-range wireless receiver gives you extended range and a stronger signal for smooth streaming even in rooms farther from your router ",
"Brilliant picture quality: Experience your favorite shows with stunning detail and clarity—whether you’re streaming in HD, 4K, or HDR, you’ll enjoy picture quality that’s optimized for your TV with sharp resolutionand vivid color ",
"Tons of power, tons of fun: Snappy and responsive, you’ll stream your favorites with ease—from movies and series on Apple TV, Disney+,Prime Video, Netflix, The Roku Channel, HBO, to cable alternatives like Sling and Hulu with Live TV, enjoy the most talked-about TV across thousands of channels ",
"No more juggling remotes: Power up your TV, adjust the volume, mute, and control your streaming all with one remote—use your voice to quickly search across channels, turn captions on, and more in a touch ",
"Setup is a cinch: Plug it in, connect to the internet, and start streaming—it’s that simple",
 "Endless entertainment: Stream what you love, including free TV, live news, sports, and more. It’s easy to stream what you love and cut back on cable bills with access to 500,000+ movies and TV episodes across thousands of free and paid channels ",
  "Private listening on mobile: Use the free Roku mobile app to pump up the volume on your shows without disturbing the house ",
   "Up to 120MB/s transfer speeds let you move up to 1000 photos in a minute (5). Up to 120MB/s read speed, engineered with proprietary technology to reach speeds beyond UHS-I 104MB/s, require compatible devices capable of reaching such speed. Write speeds lower. Based on internal testing; performance may be lower depending on host device, interface, usage conditions and other factors. 1MB=1,000,000 bytes. (5)Based on internal testing on images with an average file size of 3.55MB (up to 3.7GB total) with USB 3.0 reader. Your results will vary based on host device, file attributes, and other factors. ",
   "Load apps faster with A1-rated performance (1). (1) A1 performance is 1500 read IOPS, 500 write IOPS. Based on internal testing. Results may vary based on host device, app type and other factors. ",
   "10-year manufacturer warranty (See official SanDisk website for more details regarding warranty in your region.) ",
   "Wide Compatibility - Ender 3 Pro/Ender 3/Ender 3 V2/Ender 5/Monoprice Mini 3D Printer/Anet A8 3D Printer/Raspberry Pi/GPS/TV /SanDisk microSDHC/ Arduino / GPS / DVD / DVR / LED / LCD screen or Smartphone etc. ", "Professional service - provided 180 days no reason to return service.This must be the one which you are looking for. ",
"High quality Original Micro USB 5Pin Male to Female extension cable;micro usb male to micro usb female is 90 degree right angled connector It's a standard USB Micro-B Cable.;Compatible for : CAR GPS MP3 MP4 PDA MOBILE PHONE PMP "};
            for (int i = 0; i < 50000; i++)
            {
                ad.Category = rnd.Next(1, 4);
                ad.SubCategoryId = rnd.Next(9, 20);
                ad.Condition = rnd.Next(1, 2);
                ad.Manufacturer = rnd.Next(1, 10);
                ad.Price = rnd.Next(1, 1000);
                ad.Title = titles[rnd.Next(0, 6)];
                ad.Type = rnd.Next(1, 3);
                ad.Description = descriptions[rnd.Next(0, 12)];
                ad.Customer = rnd.Next(2, 5);
                int result = _adRepository.InsertAd(ad);
            }
            return Ok();
        }

        [HttpPut]
        [Route("/ad")]
        [Produces("application/json")]
        [Consumes("application/json")]
        public  async Task<IActionResult> UpdateAd([FromBody] Ad ad)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return BadRequest(allErrors);
            }
            var updateResult = _adRepository.UpdateAd(ad);
            if (updateResult != null)
            {
                await _NotificationHub.Clients.All.SendAsync("ReceiveWishListNotification");
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

        [HttpGet]
        [Route("/category/{id}")]
        [Produces("application/json")]
        public IActionResult GetSubCategories(int id)
        {
            if (id < 0)
            {
                return BadRequest(new { error = "wrong subId" });
            }
            var subCategories = _adRepository.GetSubCategories(id);
            if (subCategories != null)
            {
                return Json(subCategories);
            }
            return BadRequest(new { error= "something went wrong with SubCategories" });
        }

        //[Authorize]
        [HttpPut]
        [Route("/ad/image")]
        public IActionResult UpdateImage(IFormFile img,int adId)
        {
            if (img == null)
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
            SingleFileUpload(img, adId);
            if (_adRepository.UpdateAdImg(adId))
            {
                return Json(new { message="img successfully changed!!"});
            }
            return BadRequest(new {error="something went wrong with img update" });
        }

        public void SingleFileUpload(IFormFile file,int adId)
        { 
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
        }

    }
}
