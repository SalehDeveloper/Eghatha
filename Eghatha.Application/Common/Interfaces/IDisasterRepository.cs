using Eghatha.Domain.Abstractions;
using Eghatha.Domain.Disasters;

namespace Eghatha.Application.Common.Interfaces
{
    public interface IDisasterRepository: IBaseRepository<Disaster>
    {
        Task<Disaster> GetByIdWithTeamsAsync(Guid id, CancellationToken cancellationToken);

        Task<Disaster> GetByIdWithVolunteersAsync(Guid id, CancellationToken cancellationToken);


    }
}
