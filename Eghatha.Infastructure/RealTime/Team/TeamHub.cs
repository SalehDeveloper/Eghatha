using Eghatha.Application.Common.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Infastructure.RealTime.Team
{
    public class TeamHub : Hub<ITeamClient>
    {

        public const string HubUrl = "/hubs/team";


        private readonly IUser _user;
        private readonly ITeamRepository _repository;

        public TeamHub(IUser user, ITeamRepository repository)
        {
            _user = user;
            _repository = repository;
        }

        public override async Task OnConnectedAsync()
        { 
            if (_user.Id == null)
            {
                await base.OnConnectedAsync();
                return;
            }

            var team = await _repository.GetTeamForAUserAsync(_user.Id.Value , CancellationToken.None);

            if (team is not null)
            {
                await Groups.AddToGroupAsync(
                    Context.ConnectionId,
                    $"team-leader-{team.Id}");
            }

            await base.OnConnectedAsync();

        }
    }
}
