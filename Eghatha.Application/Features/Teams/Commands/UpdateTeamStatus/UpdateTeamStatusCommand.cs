using Eghatha.Domain.Teams;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Teams.Commands.UpdateTeamStatus
{
    public record UpdateTeamStatusCommand(
      Guid Id,
      TeamStatus Status
  ) : IRequest<ErrorOr<Updated>>;
}
