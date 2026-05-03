using Eghatha.Application.Common.Models;
using Eghatha.Application.Features.Volunteers.Dtos;
using Eghatha.Domain.Volunteers.Equipments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Volunteers.Queries.GetEquipments
{
    public record GetVolunteerEquipmentsQuery(
    Guid VolunteerId,
    int Page,
    int PageSize,
    EquipmentCategory? Category)
    : ICachedQuery<PaginatedList<VolunteerEquipmentDto>>
    {
        public string CachKey =>
            $"volunteer:{VolunteerId}:equipments:" +
            $"p={Page}:ps={PageSize}:" +
            $"category={Category?.Name ?? "-"}";

        public string[] Tags => ["volunteers"];

        public TimeSpan Expiration => TimeSpan.FromMinutes(5);
    }

}
