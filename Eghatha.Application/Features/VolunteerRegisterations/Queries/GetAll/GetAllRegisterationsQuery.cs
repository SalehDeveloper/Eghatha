using Eghatha.Application.Common.Models;
using Eghatha.Application.Features.VolunteerRegisterations.Dtos;
using Eghatha.Domain.VolunteerRegisterations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.VolunteerRegisterations.Queries.GetAll
{
    public record GetAllRegisterationsQuery(
    int Page,
    int PageSize,
    string? SearchTerm,
    RegisterationStatus? Status) : ICachedQuery<PaginatedList<VolunteerRegisterationDto>>
    {
        public string CachKey => $"volunteer-registrations:" +
        $"p={Page}:" +
        $"ps={PageSize}:" +
        $"q={SearchTerm ?? "-"}:" +
        $"status={Status?.Name ?? "-"}";

        public string[] Tags => ["volunteer-registrations"];

        public TimeSpan Expiration => TimeSpan.FromMinutes(10);
    }
}
