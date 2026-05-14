using Eghatha.Application.Features.Disasters.Dtos;
using Eghatha.Domain.Disasters;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Disasters.Commands.CreateDisaster
{
    public sealed record CreateDisasterCommand(
     string Title,
     string Description,
     double Latitude,
     double Longitude,
     DisasterType DisasterType,
     string? CustomTypeDescription,
     string ReporterName,
     string ReporterPhone,
     string ReporterNationalId)

     : IRequest<ErrorOr<CreateDisasterDto>>;



}
