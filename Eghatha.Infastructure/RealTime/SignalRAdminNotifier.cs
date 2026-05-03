using Eghatha.Application.Common.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Infastructure.RealTime
{
    public class SignalRAdminNotifier : IAdminNotifier
    {
        private readonly IHubContext<AdminHub , IAdminTrackingClient> _hub;

        public SignalRAdminNotifier(IHubContext<AdminHub, IAdminTrackingClient> hub)
        {
            _hub = hub;
        }

        public async Task NotifyLiveTeamLocationUpdated(Guid teamId, double latitude, double longitude, CancellationToken cancellationToken)
        {
            await _hub.Clients.Group("Admins")
                        .TeamLiveLocationUpdated(teamId, latitude, longitude);

         

        }

        public async Task NotifyNewVolunteerRegistered(Guid referenceId, string message, string url, DateTimeOffset requestedAt , CancellationToken cancellationToken )
        {
           await _hub.Clients.Group("Admins").NewVolunteerRegisterd(referenceId , message , url, requestedAt);
        }
    }
}
