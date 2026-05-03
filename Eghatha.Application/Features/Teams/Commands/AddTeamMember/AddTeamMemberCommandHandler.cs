using Eghatha.Application.Common.Authentication;
using Eghatha.Application.Common.Errors;
using Eghatha.Application.Common.Interfaces;
using Eghatha.Application.Common.Services;
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
        private readonly ICloudinaryService _cloudinaryService;



        public AddTeamMemberCommandHandler(IIdentityService identityService, IUnitOfWork unitOfWork, ITeamRepository teamRepository, TimeProvider timeProvider, HybridCache hybridCache, ICloudinaryService cloudinaryService)
        {
            _identityService = identityService;
            _unitOfWork = unitOfWork;
            _teamRepository = teamRepository;
            _timeProvider = timeProvider;
            _hybridCache = hybridCache;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<ErrorOr<Updated>> Handle(AddTeamMemberCommand request, CancellationToken cancellationToken)
        {
            var team =await _teamRepository.GetTeamByIdWithMembersAsync(request.TeamId , cancellationToken);

            if (team == null)
                return ApplicationErrors.TeamNotFound;
            
            var userExists = await _identityService.UserExistsAsync(request.Email, cancellationToken);

            if (userExists)
                return ApplicationErrors.UserWithEmailAlreadyExitst;


            var photoPath = await _cloudinaryService.UploadUserPhotoAsync(request.Email, request.photo);
            if (photoPath.IsError) return photoPath.Errors;

            var user = await _identityService.CreatUserAsync(request.FirstName , request.LastName, request.Email , request.PhoneNumber , null , photoPath.Value , Common.Models.UserCreationMode.Invited);

            if (user.IsError) 
                return user.Errors;

            await _identityService.AddUserToRoleAsync(user.Value, Domain.Identity.Role.TeamMember);
           
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
