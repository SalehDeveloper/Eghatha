using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Volunteers.Commands.IncreaseVolunteerEquipment
{
    public sealed record IncreaseVolunteerEquipmentQuantityCommand(
     Guid VolunteerId,
     Guid EquipmentId,
     int Quantity
 ) : IRequest<ErrorOr<Updated>>;
}
