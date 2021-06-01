using DypaApi.Interfaces;
using DypaApi.Models.Worker;
using DypaApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
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

        //[Authorize]
        [HttpGet]
        [Route("/owner/xorafi")]
        public IActionResult GetMyXorafia(int OwnerId)
        {
            int ownerid = 3;
            var claims = User.Claims.ToList();
            var subId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var intId = _workerRepo.GetCustomerIdFromSub(subId);
            var xorafia = _xorafiRepo.GetXorafiaByOwnerId(ownerid);
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

        [Authorize(Policy ="Admin")]
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

        [Authorize(Policy = "Admin")]
        [HttpPost]
        [Route("/subcategory")]
        public IActionResult AddSubCategory([FromBody] SubCategory subCategory)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return BadRequest(allErrors);
            }
            if (_xorafiRepo.AddSubCategory(subCategory))
            {
                return Json(new { response = "subCategory success" });
            }
            return BadRequest(new { response = "subCategory faild" });
        }
        
        [HttpGet]
        [Route("/category")]
        public IActionResult GetCategories()
        {
            return Json(_xorafiRepo.GetCategories());
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
