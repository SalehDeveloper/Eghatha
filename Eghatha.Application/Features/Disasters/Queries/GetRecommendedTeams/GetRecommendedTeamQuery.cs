using Eghatha.Application.Common.Models;
using Eghatha.Application.Features.Disasters.Dtos;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Disasters.Queries.GetRecommendedTeams
{
    public sealed record GetRecommendedTeamQuery(Guid DisasterId) : IRequest<ErrorOr<List<RecommendedTeamDto>>>;
}