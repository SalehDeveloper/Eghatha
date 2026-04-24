using Eghatha.Application.Common.Interfaces;
using Eghatha.Domain.Volunteers;
using Eghatha.Infastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Infastructure.Repositories
{
    internal class VolunteerRepository : BaseRepository<Volunteer>, IVolunteerRepository
    {
        public VolunteerRepository(AppDbContext context) : base(context)
        {
        }
    }
}
