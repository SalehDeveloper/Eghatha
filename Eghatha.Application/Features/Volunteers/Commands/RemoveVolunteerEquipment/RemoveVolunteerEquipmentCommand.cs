using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Volunteers.Commands.RemoveVolunteerEquipment
{
    public sealed record RemoveVolunteerEquipmentCommand(
      Guid VolunteerId,
      Guid EquipmentId
  ) : IRequest<ErrorOr<Deleted>>;
}
