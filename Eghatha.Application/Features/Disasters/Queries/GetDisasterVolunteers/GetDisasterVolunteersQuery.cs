using Eghatha.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Disasters.Queries.GetDisasterVolunteers
{
    public sealed record GetDisasterVolunteersQuery(Guid DisasterId , int Page , int PageSize) : IRequest<PaginatedList<DisasterVolunteerDto>>;


    public sealed record DisasterVolunteerDto(Guid Id, string Name, string Email, string PhoneNumber, string PhotoUrl , string Status);
}
