using Eghatha.Application.Common.Interfaces;
using Eghatha.Domain.Disasters;
using Eghatha.Infastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Eghatha.Infastructure.Repositories
{
    public class DisasterRepository : BaseRepository<Domain.Disasters.Disaster>, IDisasterRepository
    {
        public DisasterRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Disaster> GetByIdWithTeamsAsync(Guid id , CancellationToken cancellationToken )
        {
            return await _context.Set<Disaster>().Include(x=> x.Teams).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<Disaster> GetByIdWithVolunteersAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Set<Disaster>().Include(x => x.Volunteers).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }



    }
}