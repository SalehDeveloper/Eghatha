using Eghatha.Application.Common.Models;
using Eghatha.Application.Features.Disasters.Dtos;
using Eghatha.Application.Features.Disasters.Queries.GetDisasterVolunteers;
using Eghatha.Domain.Abstractions;
using Eghatha.Domain.Disasters;
using Eghatha.Domain.Disasters.AffectedPersons;
using Eghatha.Domain.Disasters.DisasterResources;
using Eghatha.Domain.Disasters.DisasterVolunteers;
using Eghatha.Domain.Disasters.Reports;

namespace Eghatha.Application.Common.Interfaces
{
    public interface IDisasterRepository: IBaseRepository<Disaster>
    {
        Task<Disaster> GetByIdWithTeamsAsync(Guid id, CancellationToken cancellationToken);

        Task<Disaster> GetByIdWithVolunteersAsync(Guid id, CancellationToken cancellationToken);

        Task AddVolunteersAsync(IEnumerable<DisasterVolunteer> volunteers);

        Task<Disaster> GetByIdWithTeamsAndResources(Guid id, CancellationToken cancellationToken);

        Task<Disaster> GetByIdWithResourcesAsync(Guid id, CancellationToken cancellationToken);

        Task<Disaster> GetByIdWithAffectedPersonsAsync(Guid id, CancellationToken cancellationToken);

        Task<Disaster> GetByIdWithAllDetailsAsync(Guid id, CancellationToken cancellationToken);

        Task AddResourceAsync(DisasterResource resource, CancellationToken cancellationToken);

        Task AddAffectedPersonsAsync(IEnumerable<AffectedPerson> persons, CancellationToken cancellationToken);

        Task AddReportAsync(Report report, CancellationToken cancellationToken);

        Task<PaginatedList<DisasterDto>> GetDisastersAsync(int page, int pageSize, string? city, string? province, string? type, string? status, DateTimeOffset? from, DateTimeOffset? to, CancellationToken cancellationToken);

        Task<PaginatedList<DisasterVolunteerDto>> GetDisasterVolunteersAsync(Guid disasterId, int page, int pageSize, CancellationToken cancellationToken);


    }
}
