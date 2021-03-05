  using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Controllers
{
    public class SecretController : Controller
    {
        //[Route("/secret")]
        //[Authorize(Roles="Admin")]
        [Authorize]
        public IActionResult Index()
        {
            var claims = User.Claims.ToList();
            return Json(new{ message="nasas secrets (ApiOne)"});
        }
        
        [Route("/malakas")]
        [Authorize(Roles= "malakas")]
        public IActionResult MalakasSecret()
        {
            var claims = User.Claims.ToList();
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return Json(new{ message="nasas secrets (ApiOne)"});
        }

    }
}
