using Eghatha.Application.Features.VolunteerRegisterations.Dtos;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.VolunteerRegisterations.Queries.GetById
{
    public record GetVolunteerRegisterationQueryById(Guid RegisterationId) : IRequest<ErrorOr<VolunteerRegisterationDto>>;
}
