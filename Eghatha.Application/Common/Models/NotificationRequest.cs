using Eghatha.Domain.Notifications;

namespace Eghatha.Application.Common.Models
{
    public sealed class NotificationRequest
    {
        public string Title { get; init; }

        public string Message { get; init; }

        public string? Url { get; init; }

        public NotificationType Type { get; init; }

        public IReadOnlyCollection<Guid> UserIds { get; init; }
            = [];
    }

}
