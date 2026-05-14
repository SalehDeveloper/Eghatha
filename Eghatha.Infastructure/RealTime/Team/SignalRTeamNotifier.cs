using Eghatha.Application.Common.Interfaces;
using Eghatha.Domain.Teams;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Infastructure.RealTime.Team
{
    public class SignalRTeamNotifier : ITeamNotifier
    {
        private readonly IHubContext<TeamHub, ITeamClient> _hub;

        public SignalRTeamNotifier(IHubContext<TeamHub, ITeamClient> hub)
        {
            _hub = hub;
        }

        

        public async Task NotifyTeamAssignedToDisaster(Guid teamId, string refernceId, string title, string city, string message, CancellationToken cancellationToken)
        {
            await _hub.Clients
            .Group($"team-leader-{teamId}")
            .TeamAssignedToDisaster(teamId, refernceId, title, city, message);
        }
    }
}
