using Eghatha.Application.Common.Interfaces;
using Eghatha.Domain.Disasters.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Disasters.EventHandlers
{
    public class TeamAssignedToDisasterEventHandler : INotificationHandler<TeamAssignedToDisasterEvent>
    {
        private readonly ITeamNotifier _teamNotifier;
        private readonly IAdminNotifier _adminNotifier;


        public TeamAssignedToDisasterEventHandler(ITeamNotifier teamNotifier, IAdminNotifier adminNotifier)
        {
            _teamNotifier = teamNotifier;
            _adminNotifier = adminNotifier;
        }

        public async Task Handle(TeamAssignedToDisasterEvent notification, CancellationToken cancellationToken)
        {

            await _teamNotifier.NotifyTeamAssignedToDisaster(notification.TeamId,
                $"disasters{notification.DisasterId}",
                notification.DisasterTitle,
                notification.City,
                $"Your team has been assigned to disaster '{notification.DisasterTitle}'",
                cancellationToken);


            await _adminNotifier.NotifyTeamAssignedToDisaster(notification.TeamId, notification.DisasterId, cancellationToken);
        }
    }
}
