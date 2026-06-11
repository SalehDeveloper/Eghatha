using Eghatha.Application.Common.Interfaces;
using Eghatha.Application.Features.Teams.Queries.GetTeamMemberInfo;
using MediatR;

namespace Eghatha.Application.Features.Teams.Queries.GetTeamMemberInfo
{
    public sealed record GetMyTeamQueryHandler : IRequestHandler<GetCurrentTeamMemberInfo, TeamMemberInfo>
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IUser _user;


        public GetMyTeamQueryHandler(ITeamRepository teamRepository, IUser user)
        {
            _teamRepository = teamRepository;
            _user = user;
        }

        public async Task<TeamMemberInfo> Handle(GetCurrentTeamMemberInfo request, CancellationToken cancellationToken)
        {

            var userId = _user.Id.Value;

            var response = await _teamRepository.GetCurrentTeamMemberInfo(userId, cancellationToken);

            return response;


        }





    }
}