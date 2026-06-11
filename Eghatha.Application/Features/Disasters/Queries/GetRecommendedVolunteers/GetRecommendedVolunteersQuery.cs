using Eghatha.Application.Common.Models;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Disasters.Queries.GetRecommendedVolunteers
{
    public sealed record class GetRecommendedVolunteersQuery(Guid DisasterId) : IRequest<ErrorOr<List<RecommendedVolunteerDto>>>;



}
