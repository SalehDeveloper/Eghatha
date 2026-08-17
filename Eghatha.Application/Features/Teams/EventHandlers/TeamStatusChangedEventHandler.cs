using Eghatha.Application.Common.Interfaces;
using Eghatha.Domain.Teams.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Teams.EventHandlers
{
    public class TeamStatusChangedEventHandler : INotificationHandler<TeamStatusChangedEvent> 
    {
        private readonly IAdminNotifier _adminNotifier;

        public TeamStatusChangedEventHandler(IAdminNotifier adminNotifier)
        {
            _adminNotifier = adminNotifier;
        }

        public async Task Handle(TeamStatusChangedEvent notification, CancellationToken cancellationToken)
        {
            Console.WriteLine($"[Handler] TeamStatusChangedEventHandler FIRED for {notification.TeamId} -> {notification.Status}");
           
            await _adminNotifier.NotifyTeamStatusUpdated(
                notification.TeamId,
                notification.Status.ToString(),
                cancellationToken);
        }
    }
}
