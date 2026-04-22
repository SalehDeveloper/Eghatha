using Eghatha.Application.Common.Models;
using Eghatha.Application.Features.Teams.Dtos;
using Eghatha.Domain.Teams.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Teams.Queries.GetTeamResources
{
    public record GetTeamResourcesQuery(
    Guid TeamId,
    int Page,
    int PageSize,
    Domain.Teams.Resources.ResourceType? Type)
    : ICachedQuery<PaginatedList<TeamResourceDto>>
    {
        public string CachKey =>
            $"team:{TeamId}:resources:" +
            $"p={Page}:ps={PageSize}:" +
            $"type={Type?.Name ?? "-"}";

        public string[] Tags => ["teams"];

        public TimeSpan Expiration => TimeSpan.FromMinutes(5);
    }



}
