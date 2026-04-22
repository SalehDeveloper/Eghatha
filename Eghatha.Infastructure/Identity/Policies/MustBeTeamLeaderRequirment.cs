using Eghatha.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Infastructure.Identity.Policies
{
    public class MustBeTeamLeaderRequirment : IAuthorizationRequirement
    {

    }

    public class MustBeTeamLeaderHandler : AuthorizationHandler<MustBeTeamLeaderRequirment>
    {

        private readonly IUser _user;
        private readonly ITeamRepository _teamRepository;

        public MustBeTeamLeaderHandler(IUser user, ITeamRepository teamRepository)
        {
            _user = user;
            _teamRepository = teamRepository;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, MustBeTeamLeaderRequirment requirement)
        {
            var currentUserId = _user.Id;


            var team = await _teamRepository.GetTeamForAUserAsync(currentUserId.Value, cancellationToken: CancellationToken.None);

            if (team == null)
            {
                context.Fail();

                return;

            }
            var memeber = team.Members.FirstOrDefault(m => m.Id == currentUserId.Value);

            if (memeber != null && memeber.IsLeader)
            { 

                context.Succeed(requirement);
                return;
            }

            context.Fail();

        }
    }



}
