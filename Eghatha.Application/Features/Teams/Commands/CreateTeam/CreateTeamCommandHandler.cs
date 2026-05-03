using Eghatha.Application.Common.Interfaces;
using Eghatha.Application.Common.Services;
using Eghatha.Domain.Abstractions;
using Eghatha.Domain.Shared.ValueObjects;
using Eghatha.Domain.Teams;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Teams.Commands.CreateTeam
{
    public class CreateTeamCommandHandler : IRequestHandler<CreateTeamCommand, ErrorOr<Guid>>
    {
        private readonly IUnitOfWork    _unitOfWork;
        private readonly ITeamRepository _teamRepository;
        private readonly IUser _user;
        private readonly HybridCache _hybridCache;
        private readonly IGeocodingService _geocodingService;


        public CreateTeamCommandHandler(IUnitOfWork unitOfWork, ITeamRepository teamRepository, IUser user, HybridCache hybridCache, IGeocodingService geocodingService)
        {
            _unitOfWork = unitOfWork;
            _teamRepository = teamRepository;
            _user = user;
            _hybridCache = hybridCache;
            _geocodingService = geocodingService;
        }

        public async Task<ErrorOr<Guid>> Handle(CreateTeamCommand request, CancellationToken cancellationToken)
        {
            var location = GeoLocation.Create(request.Latitude, request.Longitude);

            if (location.IsError) return location.Errors;

          //  var currentUserId = _user.Id;

            var LocationResult = await _geocodingService.ResolveAsync(request.Latitude, request.Longitude, cancellationToken);

            //var team = Team.Create(Guid.NewGuid(), request.Name, request.Speciality, LocationResult.Province, LocationResult.City, location.Value, currentUserId.Value);

            var team = Team.Create(Guid.NewGuid(), request.Name, request.Speciality, LocationResult.Province, LocationResult.City, location.Value, Guid.Parse("9668180C-06CB-43DD-8CFA-2EF9D617F47E"));
            if (team.IsError) return team.Errors;   

            await _teamRepository.AddAsync(team.Value);

            await _unitOfWork.CompleteAsync(cancellationToken);

            await _hybridCache.RemoveByTagAsync("teams" , cancellationToken);

            return team.Value.Id;
        }
    }
}
