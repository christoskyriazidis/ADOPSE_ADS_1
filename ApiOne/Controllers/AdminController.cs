using ApiOne.Hubs;
using ApiOne.Interfaces;
using ApiOne.Models.Admin;
using ApiOne.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.SignalR;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Controllers
{
    public class AdminController : Controller
    {
        private readonly IHubContext<ChatHub> _chatHub;
        private readonly IAdminRepository _adminRepository = new AdminRepository();
        private readonly IWebHostEnvironment _env;

        public AdminController(IHubContext<ChatHub> chatHub, IWebHostEnvironment webHostEnvironment)
        {
            _env = webHostEnvironment;
            _chatHub = chatHub;
        }

        [Route("/admin")]
        [Authorize(Policy = "Admin")]
        [HttpPost]
        public async Task<IActionResult> ChatAnnounce(string Message)
        {
            await _chatHub.Clients.All.SendAsync("AdminAnnounce", Message);
            return Ok();
        }
        
        [Route("/admin/category")]
        [Authorize(Policy = "Admin")]
        [HttpPost]
        public IActionResult PostCategory([FromForm]InsertCategory insertCategory)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return BadRequest(allErrors);
            }
            if (insertCategory.Img == null)
            {
                return BadRequest(new { error = "Image cannot be null(den exeis dialeksei)" });
            }
            else if (insertCategory.Img.ContentType != "image/png" && insertCategory.Img.ContentType != "image/jpeg" && insertCategory.Img.ContentType != "image/jpg")
            {
                return BadRequest(new { error = "Wrong file type (png/jpeg/jpg)" });
            }
            else if (insertCategory.Img.Length > 3145728)
            {
                return BadRequest(new { error = "File is too big (max 3mb)" });
            }
            if (_adminRepository.InsertCategory(insertCategory))
            {
                insertCategory.Title = insertCategory.Title.Replace(" ", "");
                SingleFileUpload(insertCategory.Img, insertCategory.Title);
                return Json(new { response= $"Category:{insertCategory.Title} added!"});
            }
            return BadRequest(new { response = "something went wrong"});
        }

        [Route("/admin/Subcategory")]
        [Authorize(Policy = "Admin")]
        [HttpPost]
        public IActionResult PostSubCategory([FromForm] InsertSubCategory insertSubCategory)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return BadRequest(allErrors);
            }
            if (insertSubCategory.Img == null)
            {
                return BadRequest(new { error = "Image cannot be null(den exeis dialeksei)" });
            }
            else if (insertSubCategory.Img.ContentType != "image/png" && insertSubCategory.Img.ContentType != "image/jpeg" && insertSubCategory.Img.ContentType != "image/jpg")
            {
                return BadRequest(new { error = "Wrong file type (png/jpeg/jpg)" });
            }
            else if (insertSubCategory.Img.Length > 3145728)
            {
                return BadRequest(new { error = "File is too big (max 3mb)" });
            }
            if (_adminRepository.InsertSubCategory(insertSubCategory))
            {
                insertSubCategory.Title = insertSubCategory.Title.Replace(" ", "");
                SingleFileUpload(insertSubCategory.Img, insertSubCategory.Title);
                return Json(new { response = $"Category:{insertSubCategory.Title} added!" });
            }
            return BadRequest(new { response = "something went wrong" });
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        [Route("admin/report")]
        public IActionResult GetReports()
        {
            var reports = _adminRepository.GetReports();
            if (reports != null)
            {
                return Json(reports);
            }
            return Json(new { response = "No reports yet" });
        }
        
        [HttpGet]
        [Authorize(Policy = "Admin")]
        [Route("admin/report/{AdId}")]
        public IActionResult GetReportsByAd(int AdId)
        {
            var reports = _adminRepository.GetReportsByAd(AdId);
            if (reports != null)
            {
                return Json(reports);
            }
            return Json(new { response = "No reports yet" });
        }








        public void SingleFileUpload(IFormFile file, string title)
        {

            var dir = _env.ContentRootPath;
            var smallSizeAdPath = Path.Combine(dir, "Images", "CategorySubcategory", $"{title}.png");
            using var image = Image.Load(file.OpenReadStream());
            image.Mutate(x => x.Resize(300, 300));
            image.Save(smallSizeAdPath);
        }


    }
}
