using DypaApi.Interfaces;
using DypaApi.Models.Worker;
using DypaApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DypaApi.Controllers
{
    public class OwnerController : Controller
    {
        private readonly IXorafi _xorafiRepo = new XorafiRepository();
        private readonly IWorker _workerRepo = new WorkerRepository();
        private readonly IWebHostEnvironment _env;

        public OwnerController(IWebHostEnvironment webHostEnvironment)
        {
            _env = webHostEnvironment;
        }

        //[Authorize]
        [HttpGet]
        [Route("/owner/xorafi")]
        public IActionResult GetMyXorafia(int OwnerId)
        {
            //int ownerid = 3;
            var claims = User.Claims.ToList();
            var subId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var intId = _workerRepo.GetCustomerIdFromSub(subId);
            var xorafia = _xorafiRepo.GetXorafiaByOwnerId(intId);
            if (xorafia!=null)
            {
                return Json(xorafia);
            }
            return BadRequest(new { response = "No xorafia or error" });
        }
        
        [Authorize]
        [HttpGet]
        [Route("/owner")]
        public IActionResult GetMyInfo(int OwnerId)
        {
            int ownerid = 3;
            //var claims = User.Claims.ToList();
            //var subId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            //var intId = _workerRepo.GetCustomerIdFromSub(subId);
            var info = _workerRepo.GetMyProfile(3);
            if (info != null)
            {
                return Json(info);
            }
            return BadRequest(new { response = "No info or error" });
        }

        [HttpPost]
        [Route("/category")]
        public IActionResult AddCategory([FromBody] Category category)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return BadRequest(allErrors);
            }
            if (_xorafiRepo.AddCategory(category))
            {
                return Json(new { response = "addcategory success" });
            }
            return BadRequest(new { response="addcategory faild" });
        }

        [HttpPost]
        [Route("/subcategory")]
        public IActionResult AddSubCategory([FromForm] SubCategory subCategory)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return BadRequest(allErrors);
            }
            if (subCategory.Image != null)
            {
                subCategory.ImageUrl = $"https://localhost:44331/Images/Subcategory/{subCategory.Title}.png";
                if (_xorafiRepo.AddSubCategory(subCategory))
                {
                    SingleFileUpload(subCategory.Image, subCategory.Title);
                    return Json(new { response = "subCategory success with img" });
                }
            }
            else
            {
                subCategory.ImageUrl = $"https://localhost:44331/Images/sample.png";
                if (_xorafiRepo.AddSubCategory(subCategory))
                {
                    return Json(new { response = "subCategory success default img" });
                }
            }
            return BadRequest(new { response = "subCategory faild" });
        }
        
        [HttpGet]
        [Route("/category")]
        public IActionResult GetCategories()
        {
            return Json(_xorafiRepo.GetCategories());
        }

        public void SingleFileUpload(IFormFile file, string title)
        {
            var dir = _env.ContentRootPath;
            var smallSizeAdPath = Path.Combine(dir, "Images", "Subcategory", $"{title}.png");
            using var image = Image.Load(file.OpenReadStream());
            image.Mutate(x => x.Resize(300, 300));
            image.Save(smallSizeAdPath);
            //var FullSizeAdPath = Path.Combine(dir, "Images", "serverA", "full", $"{title}.png");
            //using (var fileStream = new FileStream(FullSizeAdPath, FileMode.Create, FileAccess.Write))
            //{
            //    file.CopyTo(fileStream);
            //}
        }

        [HttpGet]
        [Route("/subcategory")]
        public IActionResult GetSubCategories()
        {
            return Json(_xorafiRepo.GetSubCategories());

        }

        [HttpPost]
        [Route("/xorafi/water/{XorafiId}")]
        public IActionResult WaterXorafi(int XorafiId)
        {
            if (_xorafiRepo.WaterXorafi(XorafiId))
            {
                return Json(new {reponse="xorafi added" });
            }
            return BadRequest(new { response = "failed" });
            
        }
    }
}
