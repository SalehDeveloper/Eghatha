using Eghatha.Application.Common.Models;
using Eghatha.Application.Features.VolunteerRegisterations.Dtos;
using Eghatha.Domain.Abstractions;
using Eghatha.Domain.VolunteerRegisterations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Common.Interfaces
{
    public interface IVolunteerRegisterationRepository : IBaseRepository<VolunteerRegisteration>
    {

        Task<PaginatedList<VolunteerRegisterationDto>> GetRegisterationsAsync(int page, int pageSize, string? SearchTerm,
            RegisterationStatus? Status, CancellationToken cancellationToken);
    }
}
