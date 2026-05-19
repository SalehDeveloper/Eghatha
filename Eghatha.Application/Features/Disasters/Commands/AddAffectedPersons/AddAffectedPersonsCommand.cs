using Eghatha.Application.Features.Disasters.Dtos;
using Eghatha.Domain.Disasters;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Disasters.Commands.AddAffectedPersons
{
    public sealed record AddAffectedPersonsCommand(  Guid DisasterId,List<AffectedPersonDto> Persons)
        : IRequest<ErrorOr<Success>>;
}
