using Eghatha.Application.Common.Models;
using Eghatha.Application.Features.Notifications.Dtos;
using Eghatha.Domain.Abstractions;
using Eghatha.Domain.Notifications;

namespace Eghatha.Application.Common.Interfaces
{
    public interface INotificationRepository : IBaseRepository<Domain.Notifications.Notification>
    {
        Task<PaginatedList<NotificationDto>> GetNotificationsByUserIdAsync(Guid userId, int page, int pageSize, CancellationToken cancellationToken);

        Task<int> GetUnreadCountAsync(Guid userId, CancellationToken cancellationToken);

        Task<NotificationRecipient> GetNotificationRecipientAsync(Guid notificationId, Guid userId, CancellationToken cancellationToken);

        Task<List<NotificationRecipient>> GetNotificationRecipientsByUserIdAsync(Guid userId, CancellationToken cancellationToken);

        Task<NotificationDto> GetNotificationByIdAsync(Guid notificationId, Guid userId, CancellationToken cancellationToken);
    }
}
