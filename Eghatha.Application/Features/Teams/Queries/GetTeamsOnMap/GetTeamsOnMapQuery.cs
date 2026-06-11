using Eghatha.Application.Features.Teams.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Teams.Queries.GetTeamsOnMap
{
    public sealed record GetTeamsOnMapQuery : IRequest<List<TeamMapDto>>;


}
