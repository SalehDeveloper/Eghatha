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
        private readonly ITeamRepository _teamtRepository;


        public ResourceDispatchedToDisasterTimeLineHandler(
            IDisasterTimeLineRepository repository, TimeProvider timeProvider, ITeamRepository teamtRepository)
        {
            _repository = repository;
            _timeProvider = timeProvider;
            _teamtRepository = teamtRepository;
        }

        public async Task Handle(ResourceDispatchedToDisaster notification, CancellationToken cancellationToken)
        {
            var team = await _teamtRepository.GetByIdAsync(notification.TeamId, cancellationToken);

            var timeline = DisasterTimeLineEvent.Create(
                notification.DisasterId,
                DisasterTimelineEventTypes.ResourceUpdated,
                $"Resource {notification.ResourceType} dispatched to team {team.Name} with quantity {notification.Quantity}",
                _timeProvider.GetUtcNow());

            await _repository.AddAsync(timeline, cancellationToken);
        }
    }


}
