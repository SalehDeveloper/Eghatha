using Eghatha.Application.Features.Notifications.Dtos;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Notifications.Queries.GetById
{
    public sealed record GetNotificationByIdQuery(Guid NotificationId) : IRequest<ErrorOr<NotificationDto>>;
}
