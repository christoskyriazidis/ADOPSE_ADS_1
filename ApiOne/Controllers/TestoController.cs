using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Controllers
{
    public class TestoController : Controller
    {

        [HttpGet]
        [Route("/testbench")]
        public IActionResult Testbench()
        {
            return Ok();
        }
    }
}
