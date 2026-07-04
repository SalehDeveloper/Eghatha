using Eghatha.Application.Common.Interfaces;
using Eghatha.Application.Common.Models;
using Eghatha.Application.Common.Services;
using Eghatha.Domain.Disasters;
using Eghatha.Domain.Disasters.Events;
using Eghatha.Domain.Notifications;
using Eghatha.Domain.Teams;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Disasters.EventHandlers
{
    internal class TeamAssignedToDisasterNotificationHandler : INotificationHandler<TeamAssignedToDisasterEvent>
    {
        private readonly ITeamRepository _teamRepository;
       

        private readonly INotificationService _notificationService;

        public TeamAssignedToDisasterNotificationHandler(ITeamRepository teamRepository, INotificationService notificationService)
        {
            _teamRepository = teamRepository;
            _notificationService = notificationService;
         
        }

        public async Task Handle(TeamAssignedToDisasterEvent notification, CancellationToken cancellationToken)
        {
            var team = await _teamRepository.GetTeamByIdWithMembersAsync(notification.TeamId, cancellationToken);
         
            var request = new NotificationRequest
            {
                Title = "New Disaster Assigned",
                Message =
                 $"A new disaster has been assigned to your team in {notification.City}",
                Url = $"/team/disasters/{notification.DisasterId}",
                UserIds = new[] { team.Leader.UserId },
                Type = NotificationType.TeamAssignedToDisaster
            };


            await _notificationService.SendAsync(
                request,
                cancellationToken);
        }
    }
}
