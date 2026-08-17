using Eghatha.Application.Common.Interfaces;
using Eghatha.Domain.Disaster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Disasters.EventHandlers
{
    public class DisasterClosedEventHandler : INotificationHandler<DisasterClosed>
    {
        private readonly IAdminNotifier _adminNotifier;

        public DisasterClosedEventHandler(IAdminNotifier adminNotifier)
        {
            _adminNotifier = adminNotifier;
        }

        public async Task Handle(DisasterClosed notification, CancellationToken cancellationToken)
        {
            Console.WriteLine($"[Handler] DisasterClosedEventHandler FIRED for {notification.Id}");
            await _adminNotifier.NotifyDisasterClosed(notification.Id, cancellationToken);
        }
    }
    
}
