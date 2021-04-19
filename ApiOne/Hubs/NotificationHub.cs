using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task SendWishListNotification(string message)
        {
            //var identity = (ClaimsIdentity)Context.User.Identity;
            await Clients.All.SendAsync("ReceiveWishListNotification");
        }
        public override async Task OnConnectedAsync()
        {
            //ConnectedUsers.Add(Context.User.FindFirst(claim => claim.Type == "username")?.Value);
            await Clients.All.SendAsync("testing", "ksupna");
            await base.OnConnectedAsync();
        }

    }
}
