using Eghatha.Application.Features.Teams.Queries.GetTeamDisasters;
using Eghatha.Application.Features.Volunteers.Dtos;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Volunteers.Queries.GetCurrentVolunteerDisaster
{
    public sealed record  GetCurrentVolunteerDisasterQuery (Guid VolunteerId): IRequest<ErrorOr<VolunteerDisastersDto>>;




}
