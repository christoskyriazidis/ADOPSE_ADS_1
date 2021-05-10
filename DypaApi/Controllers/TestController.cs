using DypaApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DypaApi.Controllers
{
    public class TestController :Controller
    {
        [Authorize]
        [Route("/test")]
        [HttpGet]
        public IActionResult TestEndPoints()
        {
            //var count = Test.Testing();
            //return Json(new {count=count });
            return Json(new { message="authorize"});
        }
    }
}
