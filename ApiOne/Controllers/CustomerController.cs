﻿using ApiOne.Interfaces;
using ApiOne.Models.Ads;
using ApiOne.Models.Customer;
using ApiOne.Models.Queries;
using ApiOne.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Nest;
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


        [HttpGet]
        [Route("/customer")]
        public IActionResult GetCustomers([FromQuery] Pagination pagination)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { error = "Ads Out of range" });
            }
            var result = _customerRepo.GetCustomers(pagination);
            if (result == null)
            {
                return BadRequest(new { error = "customer Out of range" });
            }
            return Json(result);
        }
        
        [HttpGet]
        [Route("/customer/{id}")]
        public IActionResult GetCustomer(int id)
        {
            var result = _customerRepo.GetCustomer(id);
            if (result == null)
            {
                return BadRequest(new { error = "wrong customer id" });
            }
            return Json(result);
        }

        //[Authorize]
        [HttpGet]
        [Route("/profile")]
        public IActionResult GetProfile()
        {
            int cid = 3;
            return Json(_customerRepo.GetMyProfileInfo(cid));
        }

        [HttpPut]
        [Consumes("application/json")]
        [Route("/profile")]
        public IActionResult UpdateProfile([FromBody] CustomerDetails customerDetails)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //id
            if (_customerRepo.UpdateProfile(customerDetails)) return Json(new {message="Profile Updated Succesfully" });
            else return BadRequest(new { message = "Something went wrong (profile update)" });
        }

        [HttpPut]
        [Route("/profile/image")]
        public IActionResult UpdateProfileImage([FromForm] IFormFile img)
        {
            if(img == null)
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
            var claims = User.Claims.ToList(); 
            var id = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            int uid = 3;
            if (_customerRepo.UpdateProfileImage(3))
            {
                SingleFileUpload(img, uid);
                return Json(new { message = "profile image changed!!!" });
            }
            return BadRequest(new { error = "image change failed." });

        }

        public void SingleFileUpload(IFormFile file, int userID)
        {
            var dir = _env.ContentRootPath;
            var smallSizeAdPath = Path.Combine(dir, "Images", "profile", $"{userID}.png");
            using var image = Image.Load(file.OpenReadStream());
            image.Mutate(x => x.Resize(300, 300));
            image.Save(smallSizeAdPath);
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
