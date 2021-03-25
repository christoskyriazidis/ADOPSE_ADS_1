using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            if (file.Length > 3145728)
            {
                return BadRequest(new { error = "File is too big (max 3mb)" });
            }
            if (file.ContentType != "image/png" && file.ContentType != "image/jpeg" && file.ContentType != "image/jpg")
            {
                return BadRequest(new { error = "Wrong file type" });
            }
            var userId = 4325238454;
            var dir = _env.ContentRootPath;
            var type = file.ContentType.Substring(file.ContentType.IndexOf("/")+1);
            var AdPath = Path.Combine(dir, "Images", "Ad", $"{userId}.{type}");
            using (var fileStream = new FileStream(AdPath, FileMode.Create, FileAccess.Write))
            {
                file.CopyTo(fileStream);
            }
            return Json(new { success = "profile img successfully changed" });
        }



    }
}
