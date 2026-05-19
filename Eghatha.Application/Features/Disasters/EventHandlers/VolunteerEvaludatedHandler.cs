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
    public sealed class VolunteerEvaludatedHandler : INotificationHandler<VolunteerEvaluated>
    {
        private readonly IVolunteerRepository _volunteerRepository;

        public VolunteerEvaludatedHandler(IVolunteerRepository volunteerRepository)
        {
            _volunteerRepository = volunteerRepository;
        }

        public async Task Handle(VolunteerEvaluated notification, CancellationToken cancellationToken)
        {
            var volunteer = await _volunteerRepository.GetByIdAsync(notification.VolunteerId, cancellationToken);

            volunteer.IncreaseTotalMissions();
            volunteer.AddScore(notification.TotalScore);
        }
    }
}
