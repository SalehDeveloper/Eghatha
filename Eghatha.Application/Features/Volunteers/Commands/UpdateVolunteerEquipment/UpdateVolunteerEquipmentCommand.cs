using Eghatha.Domain.Volunteers.Equipments;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Volunteers.Commands.UpdateVolunteerEquipment
{
    public sealed record UpdateVolunteerEquipmentCommand(
      Guid VolunteerId,
      Guid EquipmentId,
      string? Name,
      EquipmentCategory? Category,
      EquipmentStatus? Status,
      int? Quantity
  ) : IRequest<ErrorOr<Updated>>;
}
