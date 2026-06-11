using Eghatha.Application.Common.Models;
using Eghatha.Application.Common.Services;
using Eghatha.Application.Features.Disasters.EventHandlers;
using Eghatha.Domain.Notifications;
using Eghatha.Domain.VolunteerRegisterations.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.VolunteerRegisterations.EventHandlers
{
    public sealed class VolunteerRegisterationCreatedNotificationHandler : INotificationHandler<VolunteerRegisterationCreated>
    {
        private readonly INotificationService _notificationService;

        private readonly IIdentityCacheService _identityCacheService;

        public VolunteerRegisterationCreatedNotificationHandler(
            INotificationService notificationService,
            IIdentityCacheService identityCacheService)
        {
            _notificationService = notificationService;
          
            _identityCacheService = identityCacheService;
        }

        public async Task Handle(VolunteerRegisterationCreated notification, CancellationToken cancellationToken)
        {
            var adminIds = await _identityCacheService.GetAdminIdsAsync(cancellationToken);


         

            var request = new NotificationRequest
            {
                Title = "New Volunteer Registered",
                Message =
                    $"A new volunteer has been registered",
                Url = $"/volunteer-registrations/{notification.RegisterationId}",
                UserIds = adminIds,
                Type = NotificationType.VolunteerRegistered
            };

            await _notificationService.SendAsync(
              request,
              cancellationToken);
        }
    }
}
