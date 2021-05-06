using ApiOne.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Repositories
{
    public class AdminRepository : Controller
    {
        private readonly IHubContext<ChatHub> _chatHub;

        public AdminRepository(IHubContext<ChatHub> chatHub)
        {
            _chatHub = chatHub;
        }
        [Route("/admin")]
        [Authorize(Policy = "Admin")]
        [HttpPost]
        public async Task<IActionResult> ChatAnnounce(string Message) {
            await _chatHub.Clients.All.SendAsync("ReceiveMessage",Message);
            return Ok();
        }


    }
}
