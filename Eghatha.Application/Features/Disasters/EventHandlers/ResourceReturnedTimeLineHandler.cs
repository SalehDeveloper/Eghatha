using Eghatha.Application.Common.Interfaces;
using Eghatha.Domain.Disasters;
using Eghatha.Domain.Disasters.Events;
using MediatR;

namespace Eghatha.Application.Features.Disasters.EventHandlers
{
    public sealed class ResourceReturnedTimeLineHandler
    : INotificationHandler<ResourceReturned>
    {
        private readonly IDisasterTimeLineRepository _repository;
          private readonly TimeProvider _timeProvider;


        public ResourceReturnedTimeLineHandler(
            IDisasterTimeLineRepository repository, TimeProvider timeProvider)
        {
            _repository = repository;
            _timeProvider = timeProvider;
        }

        public Task Handle(ResourceReturned notification, CancellationToken cancellationToken)
        {
            var timeline = DisasterTimeLineEvent.Create(
                notification.DisasterId,
                DisasterTimelineEventTypes.ResourceUpdated,
                $"Returned {notification.Quantity} units from resource {notification.ResourceId}",
                _timeProvider.GetUtcNow());

            return _repository.AddAsync(timeline, cancellationToken);
        }
    }

}
