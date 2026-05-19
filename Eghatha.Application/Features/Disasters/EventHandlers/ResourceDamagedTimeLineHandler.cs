using Eghatha.Application.Common.Interfaces;
using Eghatha.Domain.Disasters;
using Eghatha.Domain.Disasters.Events;
using MediatR;

namespace Eghatha.Application.Features.Disasters.EventHandlers
{
    public sealed class ResourceDamagedTimeLineHandler
    : INotificationHandler<ResourceDamaged>
    {
        private readonly IDisasterTimeLineRepository _repository;
        private readonly TimeProvider _timeProvider;

        public ResourceDamagedTimeLineHandler(
            IDisasterTimeLineRepository repository, TimeProvider timeProvider)
        {
            _repository = repository;
            _timeProvider = timeProvider;
        }

        public Task Handle(ResourceDamaged notification, CancellationToken cancellationToken)
        {
            var timeline = DisasterTimeLineEvent.Create(
                notification.DisasterId,
                DisasterTimelineEventTypes.ResourceUpdated,
                $"Marked {notification.Quantity} units as damaged from resource {notification.ResourceId} belongs to team {notification.TeamId}",
                _timeProvider.GetUtcNow());

            return _repository.AddAsync(timeline, cancellationToken);
        }
    }

}
