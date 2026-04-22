using Eghatha.Domain.Teams;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Teams.Commands.UpdateTeam
{
    public record UpdateTeamCommand(
     Guid TeamId,
     string? Name,
     TeamSpeciality? Speciality,
     string? Province,
     string? City ) : IRequest<ErrorOr<Updated>>;



}
