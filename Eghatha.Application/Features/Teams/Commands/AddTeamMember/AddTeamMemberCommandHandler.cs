using Eghatha.Application.Common.Authentication;
using Eghatha.Application.Common.Errors;
using Eghatha.Application.Common.Interfaces;
using Eghatha.Domain.Abstractions;
using Eghatha.Domain.Teams.TeamMembers;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Teams.Commands.AddTeamMember
{
    public class AddTeamMemberCommandHandler : IRequestHandler<AddTeamMemberCommand, ErrorOr<Updated>>
    {
        private readonly IIdentityService _identityService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITeamRepository _teamRepository;
        private readonly TimeProvider _timeProvider;
        private readonly HybridCache _hybridCache;


        public AddTeamMemberCommandHandler(IIdentityService identityService, IUnitOfWork unitOfWork, ITeamRepository teamRepository, TimeProvider timeProvider, HybridCache hybridCache)
        {
            _identityService = identityService;
            _unitOfWork = unitOfWork;
            _teamRepository = teamRepository;
            _timeProvider = timeProvider;
            _hybridCache = hybridCache;
        }

        public async Task<ErrorOr<Updated>> Handle(AddTeamMemberCommand request, CancellationToken cancellationToken)
        {
            var team =await _teamRepository.GetTeamByIdWithMembersAsync(request.TeamId , cancellationToken);

            if (team == null)
                return ApplicationErrors.TeamNotFound;

            var user = await _identityService.CreatUserAsync(request.FirstName , request.LastName, request.Email , request.PhoneNumber , null , request.PhotoUrl , Common.Models.UserCreationMode.Invited);

            if (user.IsError) 
                return user.Errors;

            var memeber = team.AddMember(user.Value, request.JobTitle, request.IsLeader, _timeProvider.GetUtcNow());

            if (memeber.IsError)
                return memeber.Errors;

            await _teamRepository.AddTeamMemberAsync(memeber.Value, cancellationToken);
            
            await _unitOfWork.CompleteAsync(cancellationToken);

            await _hybridCache.RemoveByTagAsync("teams");
            return Result.Updated;

        }
    }
}
