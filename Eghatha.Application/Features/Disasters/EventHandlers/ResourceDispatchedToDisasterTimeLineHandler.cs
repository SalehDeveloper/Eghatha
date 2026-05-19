using Eghatha.Application.Common.Interfaces;
using Eghatha.Domain.Disasters;
using Eghatha.Domain.Disasters.Events;
using MediatR;

namespace Eghatha.Application.Features.Disasters.EventHandlers
{
    public sealed class ResourceDispatchedToDisasterTimeLineHandler
    : INotificationHandler<ResourceDispatchedToDisaster>
    {
        private readonly IDisasterTimeLineRepository _repository;
        private readonly TimeProvider _timeProvider;


        public ResourceDispatchedToDisasterTimeLineHandler(
            IDisasterTimeLineRepository repository, TimeProvider timeProvider)
        {
            _repository = repository;
            _timeProvider = timeProvider;
        }

        public Task Handle(ResourceDispatchedToDisaster notification, CancellationToken cancellationToken)
        {
            var timeline = DisasterTimeLineEvent.Create(
                notification.DisasterId,
                DisasterTimelineEventTypes.ResourceUpdated,
                $"Resource {notification.ResourceId} dispatched to team {notification.TeamId} with quantity {notification.Quantity}",
                _timeProvider.GetUtcNow());

            return _repository.AddAsync(timeline, cancellationToken);
        }
    }


}
