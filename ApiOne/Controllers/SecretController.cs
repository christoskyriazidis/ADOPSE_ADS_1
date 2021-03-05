using ApiOne.Databases;
using ApiOne.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
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


        Database dbdb = Database.GetInstance();
        [Route("/test")]
        public  IActionResult Test()
        {
            dbdb.getprodd();
            return Json(new { testing="whatever"});
        }

    }
}
