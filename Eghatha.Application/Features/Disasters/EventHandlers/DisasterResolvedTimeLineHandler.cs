using Eghatha.Application.Common.Interfaces;
using Eghatha.Domain.Disaster;
using Eghatha.Domain.Disasters;
using MediatR;

namespace Eghatha.Application.Features.Disasters.EventHandlers
{
    public sealed class DisasterResolvedTimeLineHandler
    : INotificationHandler<DisasterResolved>
    {
        private readonly IDisasterTimeLineRepository _repository;

        public DisasterResolvedTimeLineHandler(IDisasterTimeLineRepository repository)
        {
            _repository = repository;
        }

        public Task Handle(DisasterResolved notification, CancellationToken cancellationToken)
        {
            var timeline = DisasterTimeLineEvent.Create(
                notification.Id,
                DisasterTimelineEventTypes.Resolved,
                $"Disaster was resolved at {notification.ResolvedAt:u}",
                notification.ResolvedAt);

            return _repository.AddAsync(timeline, cancellationToken);
        }
    }
}
