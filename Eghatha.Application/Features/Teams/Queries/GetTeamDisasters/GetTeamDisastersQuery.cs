using Eghatha.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Teams.Queries.GetTeamDisasters
{
    public sealed record GetTeamDisastersQuery(
     Guid TeamId,
     int Page,
    int PageSize) : ICachedQuery<PaginatedList<TeamDisastersDto>>
    {
        public string CachKey => $"team:{TeamId}:disasters:" +
            $"p={Page}:ps={PageSize}:";

        public string[] Tags => ["teams"];

        public TimeSpan Expiration => TimeSpan.FromMinutes(5);
    }

    public sealed record TeamDisastersDto(Guid DisasterId , string Title,
        string City,
        string Province,
        double Latitude,
        double Longitude,
        string Type,
        string Status,
       DateTimeOffset StartTime);


}
