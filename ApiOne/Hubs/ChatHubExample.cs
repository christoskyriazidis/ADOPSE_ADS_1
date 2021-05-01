using ApiOne.Helpers;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiOne.Hubs {
    public class ChatHubExample : Hub {
        private static ChatConnectionHelper2 connectionHelper = new ChatConnectionHelper2();

        public override Task OnConnectedAsync() {
            string username = Context.User.FindFirst(claim => claim.Type == "username")?.Value;
            connectionHelper.Add(username, Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception) {
            string username = Context.User.FindFirst(claim => claim.Type == "username")?.Value;
            connectionHelper.Remove(username, Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendPrivateMessage(string message,string receiver) {
            string sender = Context.User.FindFirst(claim => claim.Type == "username")?.Value;
            connectionHelper.ConnectionsOf(receiver).ForEachDo(connectionId => {
                Clients.Client(connectionId).SendAsync("ReceiveMessage", sender,message);
            });
        }
    }
}
