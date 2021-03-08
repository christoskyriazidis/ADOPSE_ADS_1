using ApiOne.Hubs;
using ApiOne.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Controllers
{
    public class UserController: Controller
    {
        private readonly IHubContext<ChatHub> _myHub;

        public UserController(IHubContext<ChatHub> hubContext)
        {
            _myHub = hubContext;
        }

        
        [HttpGet]
        [Route("/customer")]
        public JsonResult Customer()
        {
            //pernei to id tou user apo 
            var claims = User.Claims.ToList();
            var id = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            List<Ad> ads = new List<Ad>();
            ads.Add(new Ad("title",11,11,11,"desccccccccc"));
            
            return Json(ads);
        }

        [Authorize(Roles ="Admin")]
        [HttpDelete]
        [Route("/customer")]
        public async Task<IActionResult> DeleteCustomer()
        {
            //an paei kati la8os
            //return BadRequest();
            return Ok();
        }

    }
}
