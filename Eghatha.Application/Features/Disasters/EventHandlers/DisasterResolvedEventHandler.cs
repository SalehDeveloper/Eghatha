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
    public class DisasterResolvedEventHandler : INotificationHandler<DisasterResolved>
    {
        private readonly IAdminNotifier _adminNotifier;

        public DisasterResolvedEventHandler(IAdminNotifier adminNotifier)
        {
            _adminNotifier = adminNotifier;
        }

        public async Task Handle(DisasterResolved notification, CancellationToken cancellationToken)
        {
            Console.WriteLine($"disaster resolved event FIRED for {notification.Id}");
            await _adminNotifier.NotifyDisasterResolved(
                notification.Id,
                notification.ResolvedAt,
                cancellationToken);
        }
    }
}
