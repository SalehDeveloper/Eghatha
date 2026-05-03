using Eghatha.Domain.Volunteers.Equipments;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Volunteers.Commands.UpdateEquipmentStatus
{
    public sealed record UpdateVolunteerEquipmentStatusCommand(
       Guid VolunteerId,
       Guid EquipmentId,
       EquipmentStatus Status
   ) : IRequest<ErrorOr<Updated>>;
}
