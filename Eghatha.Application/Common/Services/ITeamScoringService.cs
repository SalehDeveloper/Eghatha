using Eghatha.Application.Common.Models;
using Eghatha.Domain.Disasters;
using Eghatha.Domain.Teams;

namespace Eghatha.Application.Common.Services
{
    public interface ITeamScoringService
    {
        double Calculate(
            Team team,
            Disaster disaster,
            RouteResult route);
    }
}
