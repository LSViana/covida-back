using Covida.Core.Domain;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Covida.Web.Hubs
{
    public class MessagesHub : Hub
    {
        public const string Url = "/api/v1/messages";
        public const string VolunteerGroupName = "volunteers";
        public const string GroupOfRiskGroupName = "groupOfRisk";

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public async Task SetIsVolunteer(bool isVolunteer)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, isVolunteer ? VolunteerGroupName : GroupOfRiskGroupName);
        }
    }
}
