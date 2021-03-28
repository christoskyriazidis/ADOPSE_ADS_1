using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    public class CustomerController : Controller
    {
        private readonly IWebHostEnvironment _env;

        public CustomerController(IWebHostEnvironment webHostEnvironment)
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

    }
}
