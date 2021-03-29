using ApiOne.Interfaces;
using ApiOne.Models.Queries;
using ApiOne.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Controllers
{
    public class CustomerController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly ICustomerRepository _customerRepo = new CustomerRepository();


        public CustomerController(IWebHostEnvironment webHostEnvironment )
        {
            _env = webHostEnvironment;
        }

        [HttpPost]
        [Route("/customer/profileImage")]
        public IActionResult SingleFileUpload(IFormFile file)
        {
            int adId = 5873458;
            if (file.Length > 3145728)
            {
                return BadRequest(new { error = "File is too big (max 3mb)" });
            }
            if (file.ContentType != "image/png" && file.ContentType != "image/jpeg" && file.ContentType != "image/jpg")
            {
                return BadRequest(new { error = "Wrong file type" });
            }
            var dir = _env.ContentRootPath;
            var smallSizeAdPath = Path.Combine(dir, "Images", "serverA", "small", $"{adId}.png");
            using var image = Image.Load(file.OpenReadStream());
            image.Mutate(x => x.Resize(200, 200));
            image.Save(smallSizeAdPath);
            var FullSizeAdPath = Path.Combine(dir, "Images", "serverA", "full", $"{adId}.png");
            using (var fileStream = new FileStream(FullSizeAdPath, FileMode.Create, FileAccess.Write))
            {
                file.CopyTo(fileStream);
            }
            return Ok();
        }


        [HttpGet]
        [Route("/customer")]
        public IActionResult GetCustomers([FromQuery] Pagination pagination)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { error = "Ads Out of range" });
            }
            var customers = _customerRepo.GetCustomers(pagination);
            if (customers==null)
            {
                return BadRequest(new { error = "customer Out of range" });
            }
            return Json(customers);
        }









        [HttpGet]
        [Route("/secret")]
        [Authorize(Policy = "Admin")]
        public IActionResult Secret()
        {
            var claims = User.Claims.ToList();
            var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.DateOfBirth)?.Value;
            var role = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            var id = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var username = claims.FirstOrDefault(c => c.Type == "username")?.Value;
            var usernamee = claims.FirstOrDefault(c => c.Type == "usernameeee")?.Value;

            return Json(new { secret = "very secret" });
        }
    }
}
