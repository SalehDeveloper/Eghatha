using Eghatha.Application.Common.Models;
using Eghatha.Application.Features.Volunteers.Dtos;
using Eghatha.Domain.Volunteers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Volunteers.Queries.GetTopVolunteers
{
    public sealed record GetTopVolunteersQuery(
     int Page,
     int PageSize,
     string? Province,
     string? City,
     string? Speciality,
     double? MinAverageScore,
     VolunteerRankingSortBy SortBy = VolunteerRankingSortBy.AverageScore,
     bool Descending = true)
     : ICachedQuery<PaginatedList<VolunteerRankingDto>>
    {
        public string CachKey =>
            $"volunteer-ranking:" +
            $"p={Page}:" +
            $"ps={PageSize}:" +
            $"province={Province ?? "-"}:" +
            $"city={City ?? "-"}:" +
            $"speciality={Speciality ?? "-"}:" +
            $"minScore={MinAverageScore?.ToString() ?? "-"}:" +
            $"sort={SortBy}:" +
            $"desc={Descending}";

        public string[] Tags => ["volunteer-ranking"];

        public TimeSpan Expiration => TimeSpan.FromMinutes(10);
    }

    public enum VolunteerRankingSortBy
    {
        AverageScore = 1,
        TotalMissions = 2,
        TotalScore = 3
    }
}
