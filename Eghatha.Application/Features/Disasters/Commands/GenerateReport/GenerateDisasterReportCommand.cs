using Eghatha.Application.Features.Disasters.Dtos;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Disasters.Commands.GenerateReport
{
    public sealed record GenerateDisasterReportCommand(Guid DisasterId) : IRequest<ErrorOr<GenerateDisasterReportDto>>;
    
    
}
