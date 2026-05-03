using Eghatha.Domain.Shared.ValueObjects;
using Eghatha.Domain.Teams;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Teams.Commands.CreateTeam
{
    public record CreateTeamCommand(string Name, TeamSpeciality Speciality,  double Latitude , double Longitude) : IRequest<ErrorOr<Guid>>;
    
    
}
