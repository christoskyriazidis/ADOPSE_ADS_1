using ApiOne.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Controllers
{
    public class AdminController : Controller
    {
        private readonly IHubContext<ChatHub> _chatHub;

        public AdminController(IHubContext<ChatHub> chatHub)
        {
            _chatHub = chatHub;
        }
        [Route("/admin")]
        [Authorize(Policy = "Admin")]
        [HttpPost]
        public async Task<IActionResult> ChatAnnounce(string Message)
        {
            await _chatHub.Clients.All.SendAsync("AdminAnnounce", Message);
            return Ok();
        }


    }
}
