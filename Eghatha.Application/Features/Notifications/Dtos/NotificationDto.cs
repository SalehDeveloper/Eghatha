using Eghatha.Domain.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Notifications.Dtos
{
    public sealed record NotificationDto
    {
        public Guid Id { get; init; }

        public string Title { get; init; }

        public string Message { get; init; }

        public string Url { get; init; }

        public NotificationType Type { get; init; }

        public bool IsRead { get; init; }

        public DateTimeOffset CreatedAt { get; init; }
    }
}
