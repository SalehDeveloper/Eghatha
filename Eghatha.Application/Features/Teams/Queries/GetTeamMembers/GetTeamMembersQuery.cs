using Eghatha.Application.Common.Models;
using Eghatha.Application.Features.Teams.Dtos;
using Eghatha.Domain.Teams.TeamMembers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Teams.Queries.GetTeamMembers
{
    public record GetTeamMembersQuery(
      Guid TeamId,
      int Page,
      int PageSize,
      string? SearchTerm,
      TeamMemberStatus? Status)
      : ICachedQuery<PaginatedList<TeamMemberDto>>
    {
        public string CachKey =>
            $"team:{TeamId}:members:" +
            $"p={Page}:ps={PageSize}:" +
            $"q={SearchTerm ?? "-"}:" +
            $"status={Status?.Name ?? "-"}";

        public string[] Tags => ["teams"];

        public TimeSpan Expiration => TimeSpan.FromMinutes(5);
    }
}
