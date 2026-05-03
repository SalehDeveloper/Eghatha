using Eghatha.Application.Common.Models;
using Eghatha.Application.Features.Volunteers.Dtos;
using Eghatha.Domain.Abstractions;
using Eghatha.Domain.Volunteers;
using Eghatha.Domain.Volunteers.Equipments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Common.Interfaces
{
    public interface IVolunteerRepository : IBaseRepository<Volunteer>
    {
       Task<PaginatedList<VolunteerDto>> GetVolunteersAsync(
       int page,
       int pageSize,
       string? searchTerm,
       VolunteerStatus? status,
       VolunteerSpeciality? speciality,
       string? province,
       string? city,
       CancellationToken cancellationToken);

        Task<VolunteerDto> GetVolunteerDetailsByIdAsync(Guid id, CancellationToken cancellationToken);

        Task<Volunteer> GetByIdWithEquipmentsAsync(Guid id, CancellationToken cancellationToken);

        Task AddEquipmentAsync(Equipment equipment, CancellationToken cancellationToken);

        Task<PaginatedList<VolunteerEquipmentDto>> GetVolunteerEquipmentsAsync(Guid volunteerId, int page, int pageSize, EquipmentCategory? category, CancellationToken cancellationToken);
    }
}
