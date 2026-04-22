using Eghatha.Application.Common.Errors;
using Eghatha.Application.Common.Interfaces;
using Eghatha.Application.Common.Services;
using Eghatha.Domain.Shared.ValueObjects;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Teams.Commands.UpdateLiveTeamLocation
{
    public class UpdateLiveTeamLocationCommandHandler : IRequestHandler<UpdateLiveTeamLocationCommand, ErrorOr<Updated>>
    {
        private readonly IAdminNotifier _notifier;
        private readonly ITeamRepository _teamRepository;
        private readonly ITeamLocationService _locationService;


        public UpdateLiveTeamLocationCommandHandler(IAdminNotifier notifier, ITeamRepository teamRepository, ITeamLocationService locationService)
        {
            _notifier = notifier;
            _teamRepository = teamRepository;
            _locationService = locationService;
        }

        public async Task<ErrorOr<Updated>> Handle(UpdateLiveTeamLocationCommand request, CancellationToken cancellationToken)
        { 

            var team = await _teamRepository.GetByIdAsync(request.TeamId, cancellationToken);

            if (team == null)
                return ApplicationErrors.TeamNotFound;

            var location = GeoLocation.Create(request.Latitude, request.Longitude);

            await _locationService.SetLocationAsync(request.TeamId, location.Value);


            await _notifier.NotifyLiveTeamLocationUpdated(request.TeamId, request.Latitude, request.Longitude, cancellationToken);


            return Result.Updated;
        }
    }
}
