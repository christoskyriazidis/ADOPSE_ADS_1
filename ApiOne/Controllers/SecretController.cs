using ApiOne.Databases;
using ApiOne.Hubs;
using ApiOne.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base.EventArgs;

namespace ApiOne.Controllers
{

    public class SecretController : Controller
    {
        private readonly IHubContext<ChatHub> _myHub;

        public SecretController(IHubContext<ChatHub> hubContext)
        {
            _myHub = hubContext;
        }


        [Route("/myget")]
        //[Authorize]
        [HttpGet]
        [Produces("application/json")]
        public Product GetT()
        {
            Product product = new Product("products",1,20,40,"blablabla"); 

            return product;
        }
        

        [HttpGet]
        [Route("/secret")]
        [Authorize(Policy= "Admin")]
        public IActionResult Secret()
        {
            var claims = User.Claims.ToList();
            var email = claims.FirstOrDefault(c=>c.Type == ClaimTypes.DateOfBirth)?.Value;
            var role = claims.FirstOrDefault(c=>c.Type == ClaimTypes.Role)?.Value;
            var id = claims.FirstOrDefault(c=>c.Type == ClaimTypes.NameIdentifier)?.Value;
            var username = claims.FirstOrDefault(c=>c.Type == "username")?.Value;
            var usernamee = claims.FirstOrDefault(c=>c.Type == "usernameeee")?.Value;

            return Json(new { secret ="very secret" });
        }



    }
}
