using Eghatha.Application.Common.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Infastructure.RealTime.Admin
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

        public async Task NotifyNewDisasterReported(Guid referenceId, string message, double latitude, double longitude, string url, DateTimeOffset createdAt, CancellationToken cancellationToken)
        {
           await _hub.Clients.Group("Admins").NewDisasterReported(referenceId, message, latitude, longitude, url, createdAt);
        }

        public async Task NotifyNewVolunteerRegistered(Guid referenceId, string message, string url, DateTimeOffset requestedAt , CancellationToken cancellationToken )
        {
           await _hub.Clients.Group("Admins").NewVolunteerRegisterd(referenceId , message , url, requestedAt);
        }


        public async Task NotifyDisasterResolved(Guid disasterId, DateTimeOffset resolvedAt, CancellationToken cancellationToken)
        {
            await _hub.Clients.Group("Admins").DisasterResolved(disasterId, resolvedAt);
        }

        public async Task NotifyTeamStatusUpdated(Guid teamId, string status, CancellationToken cancellationToken)
        {
            await _hub.Clients.Group("Admins").TeamStatusUpdated(teamId, status);
        }

        public async Task NotifyDisasterClosed(Guid disasterId, CancellationToken cancellationToken)
        {
            await _hub.Clients.Group("Admins").DisasterClosed(disasterId);
        }

        public async Task NotifyTeamAssignedToDisaster(Guid teamId, Guid disasterId, CancellationToken cancellationToken)
        {
            await _hub.Clients.Group("Admins").TeamAssignedToDisaster(teamId, disasterId);
        }
    }
}
