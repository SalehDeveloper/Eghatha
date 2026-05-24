using Eghatha.Domain.Notifications;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Notifications.Commands.MarkNotificationAsRead
{
    public sealed record MarkNotificationAsReadCommand(Guid NotificationId) : IRequest<ErrorOr<Updated>>;
}
