using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Covida.Web.Hubs
{
    public class MessagesHub : Hub
    {
        public const string Url = "/api/messages";

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public async Task SendMessage(string message)
        {
            await Clients.All.SendCoreAsync("ReceiveMessage", new[] { message });
        }
    }
}
