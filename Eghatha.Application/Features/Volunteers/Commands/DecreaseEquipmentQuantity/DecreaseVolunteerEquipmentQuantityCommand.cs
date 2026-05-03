using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Volunteers.Commands.DecreaseEquipmentQuantity
{
    public sealed record DecreaseVolunteerEquipmentQuantityCommand(
      Guid VolunteerId,
      Guid EquipmentId,
      int Quantity
  ) : IRequest<ErrorOr<Updated>>;
}
