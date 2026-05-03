using Eghatha.Application.Common.Models;
using Eghatha.Application.Features.Volunteers.Dtos;
using Eghatha.Domain.Volunteers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Volunteers.Queries.GetAll
{
    public record GetAllVolunteersQuery(int Page,
    int PageSize,
    string? SearchTerm,
    VolunteerStatus? Status,
    VolunteerSpeciality? Speciality,
    string? Province,
    string? City)
    : ICachedQuery<PaginatedList<VolunteerDto>>
    {
        public string CachKey =>
         $"volunteers:" +
         $"p={Page}:" +
         $"ps={PageSize}:" +
         $"q={SearchTerm ?? "-"}:" +
         $"status={Status?.Name ?? "-"}:" +
         $"speciality={Speciality?.Name ?? "-"}:" +
         $"province={Province ?? "-"}:" +
         $"city={City ?? "-"}";

        public string[] Tags => ["volunteers"];

        public TimeSpan Expiration => TimeSpan.FromMinutes(10);
    }
}
