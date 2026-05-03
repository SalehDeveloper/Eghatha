using Eghatha.Application.Common.Interfaces;
using Eghatha.Domain.VolunteerRegisterations.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.VolunteerRegisterations.EventHandlers
{
    public class VolunteerRegisterationCreatedEventHandler : INotificationHandler<VolunteerRegisterationCreated>
    {
        private readonly IAdminNotifier _adminNotifier;

        public VolunteerRegisterationCreatedEventHandler(IAdminNotifier adminNotifier)
        {
            _adminNotifier = adminNotifier;
        }

        public async Task Handle(VolunteerRegisterationCreated notification, CancellationToken cancellationToken)
        {
            await _adminNotifier.NotifyNewVolunteerRegistered(notification.RegisterationId,
                $"a new volunteer has registered",
                $"/volunteer-registerations/{notification.RegisterationId}",
                notification.CreatedAt , 
                cancellationToken );
        }
    }
}
