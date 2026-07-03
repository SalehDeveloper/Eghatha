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
        private readonly ITeamRepository _teamRepository;


        public ResourceReturnedTimeLineHandler(
            IDisasterTimeLineRepository repository, TimeProvider timeProvider, ITeamRepository teamRepository)
        {
            _repository = repository;
            _timeProvider = timeProvider;
            _teamRepository = teamRepository;
        }

        public async Task Handle(ResourceReturned notification, CancellationToken cancellationToken)
        {
            var team = await _teamRepository.GetByIdAsync(notification.TeamId, cancellationToken);


            var timeline = DisasterTimeLineEvent.Create(
                notification.DisasterId,
                DisasterTimelineEventTypes.ResourceUpdated,
                $"Returned {notification.Quantity} units from resource {notification.ResourceType} Belongs to Team {team.Name}",
                _timeProvider.GetUtcNow());

            await _repository.AddAsync(timeline, cancellationToken);
        }
    }

}
