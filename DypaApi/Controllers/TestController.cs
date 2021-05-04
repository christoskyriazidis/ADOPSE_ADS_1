using DypaApi.Repositories;
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
        [Route("/")]
        public IActionResult TestEndPoints()
        {
            var count = Test.Testing();
            return Json(new {count=count });
        }
    }
}
