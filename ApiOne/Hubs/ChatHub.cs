using ApiOne.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ApiOne.Hubs
{
    //[Authorize]
    public class ChatHub : Hub
    {

        public readonly static ChatConnectionHelper<string> _connections = new ChatConnectionHelper<string>();

        [Authorize(Policy  = "Admin")]
        public async Task SendMessage(string message)
        {
            var identity = (ClaimsIdentity)Context.User.Identity;
            string userId = Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            string username = Context.User.FindFirst(claim=>claim.Type=="username")?.Value;
            string test = Context.User.Identity.Name;
            //html 
            //var claims = User.Claims.ToList();
            message = HttpUtility.HtmlEncode(message);
            await Clients.All.SendAsync("ReceiveMessage", username, message);
        }

        public  async Task IamTyping()
        {
            string username = Context.User.FindFirst(claim => claim.Type == "username")?.Value;
            //await Clients.All.SendAsync("Typing", username);
            await Clients.AllExcept(Context.ConnectionId).SendAsync("Typing", username);
        }

        public override async Task OnConnectedAsync()
        {
            string username = Context.User.FindFirst(claim => claim.Type == "username")?.Value;
            _connections.Add(username, Context.ConnectionId);
            await Clients.All.SendAsync("ConnectToChatHub", _connections.ToString(username));
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception ex)
        {
            string username = Context.User.FindFirst(claim => claim.Type == "username")?.Value;
            _connections.Remove(username, Context.ConnectionId);
            await base.OnDisconnectedAsync(ex);
        }

        public async Task SendPrivateMessage(string message)
        {
            var identity = (ClaimsIdentity)Context.User.Identity;
            string userId = Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            string username = Context.User.FindFirst(claim => claim.Type == "username")?.Value;
            string test = Context.User.Identity.Name;
            //html 
            //var claims = User.Claims.ToList();
            message = HttpUtility.HtmlEncode(message);
            await Clients.All.SendAsync("ReceiveMessage", username, message);
        }

    }
}
