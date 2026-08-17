using Eghatha.Application.Features.Disasters.Dtos;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Disasters.Queries.CheckDisasterSpam
{
    public record CheckDisasterSpamQuery(Guid DisasterId, double RadiusKm = 2, int WindowMinutes = 20)
        : IRequest<ErrorOr<SpamCheckResultDto>>;
}
