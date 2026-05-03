using Eghatha.Domain.Volunteers.Equipments;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Volunteers.Commands.AddVolunteerEquipment
{
    public sealed record AddVolunteerEquipmentCommand(
      Guid VolunteerId,
      string Name,
      EquipmentCategory Category,
      int Quantity
  ) : IRequest<ErrorOr<Updated>>;
}
