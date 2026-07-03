using Eghatha.Application.Features.Disasters.Dtos;
using Eghatha.Domain.Disasters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Common.Services
{
    public interface  IDisasterReportPdfService
    {
        Task<byte[]> Generate(Disaster disaster, List<DisasterVolunteerReportDto> volunteers, CancellationToken cancellationToken);
    }
}
