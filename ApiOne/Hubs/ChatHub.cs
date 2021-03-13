using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        public static HashSet<string> ConnectedUsers = new HashSet<string>();


        [Authorize(Policy  = "Admin")]
        public async Task SendMessage(string message)
        {
            var identity = (ClaimsIdentity)Context.User.Identity;
            string userId = Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            string username = Context.User.FindFirst(claim=>claim.Type=="username")?.Value;
            string test = Context.User.Identity.Name;
            //var claims = User.Claims.ToList();
            await Clients.AllExcept(Context.ConnectionId).SendAsync("ReceiveMessage", username, message);
            
        }

        public  async Task IamTyping()
        {
            string username = Context.User.FindFirst(claim => claim.Type == "username")?.Value;

            //await Clients.All.SendAsync("Typing", username);
            await Clients.AllExcept(Context.ConnectionId).SendAsync("Typing", username);
        }

        public override async Task OnConnectedAsync()
        {
            ConnectedUsers.Add(Context.User.FindFirst(claim => claim.Type == "username")?.Value);
            await Clients.All.SendAsync("OnlineUsers", ConnectedUsers);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception ex)
        {
            ConnectedUsers.Remove(Context.User.FindFirst(claim => claim.Type == "username")?.Value);
            await Clients.All.SendAsync("OnlineUsers", ConnectedUsers);
            await base.OnDisconnectedAsync(ex);
        }


    }
}
