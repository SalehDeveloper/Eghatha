using Eghatha.Application.Common.Interfaces;
using Eghatha.Domain.Identity;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Infastructure.RealTime
{
    public class AdminHub : Hub<IAdminTrackingClient>
    {
        public const string HubUrl = "/hubs/admin";

        public override async Task OnConnectedAsync()
        {
            if (Context.User?.IsInRole("Admin") == true)
                await Groups.AddToGroupAsync(Context.ConnectionId, "Admins");

            await base.OnConnectedAsync();
        }
    }
}
