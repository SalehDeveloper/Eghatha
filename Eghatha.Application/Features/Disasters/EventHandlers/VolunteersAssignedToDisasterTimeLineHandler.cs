using Eghatha.Application.Common.Interfaces;
using Eghatha.Domain.Disasters;
using Eghatha.Domain.Disasters.Events;
using MediatR;

namespace Eghatha.Application.Features.Disasters.EventHandlers
{
    public sealed class VolunteersAssignedToDisasterTimeLineHandler
    : INotificationHandler<VolunteersAssignedToDisaster>
    {
        private readonly IDisasterTimeLineRepository _repository;
        private readonly TimeProvider _timeProvider;


        public VolunteersAssignedToDisasterTimeLineHandler(IDisasterTimeLineRepository repository, TimeProvider timeProvider)
        {
            _repository = repository;
            _timeProvider = timeProvider;
        }

        public Task Handle(VolunteersAssignedToDisaster notification, CancellationToken cancellationToken)
        {
            var timeline = DisasterTimeLineEvent.Create(
                notification.DisasterId,
                DisasterTimelineEventTypes.ResourceUpdated,
                $"{notification.VolunteerIds.Count} volunteers assigned to disaster in {notification.City}",
                _timeProvider.GetUtcNow());

            return _repository.AddAsync(timeline, cancellationToken);
        }
    }
}
