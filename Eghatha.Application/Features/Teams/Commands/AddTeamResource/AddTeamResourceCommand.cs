using Eghatha.Domain.Teams.TeamResources;
using ErrorOr;
using MediatR;

namespace Eghatha.Application.Features.Teams.Commands.AddTeamResource
{
    public record AddTeamResourceCommand(Guid TeamId, ResourceType Type, int Quantity):IRequest<ErrorOr<Updated>>;

}
