using Eghatha.Application.Common.Models;
using Eghatha.Application.Features.Teams.Dtos;
using Eghatha.Domain.Teams;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Teams.Queries.GetTeams
{
    public record GetTeamsQuery(int Page, int PageSize, string? SearchTerm, TeamStatus? Status, TeamSpeciality? Speciality, string? Province, string? City)
        : ICachedQuery<PaginatedList<TeamDto>>
    {
        public string CachKey =>
      $"teams:" +
      $"p={Page}:" +
      $"ps={PageSize}:" +
      $"q={SearchTerm ?? "-"}:" +
      $"status={Status?.Name ?? "-"}:" +
      $"speciality={Speciality?.Name ?? "-"}:" +
      $"province={Province ?? "-"}:" +
      $"city={City ?? "-"}";

        public string[] Tags => ["teams"];

        public TimeSpan Expiration =>TimeSpan.FromMinutes(10);
    }
}
