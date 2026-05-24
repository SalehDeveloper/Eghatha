using Eghatha.Application.Common.Interfaces;
using Eghatha.Application.Common.Models;
using Eghatha.Application.Features.Notifications.Dtos;
using MediatR;

namespace Eghatha.Application.Features.Notifications.Queries.GetNotifications
{
    public sealed class GetNotificationsQueryHandler : IRequestHandler<GetNotificationsQuery, PaginatedList<NotificationDto>>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUser _user;

        public GetNotificationsQueryHandler(INotificationRepository notificationRepository, IUser user)
        {
            _notificationRepository = notificationRepository;
            _user = user;
        }
        public async Task<PaginatedList<NotificationDto>> Handle(GetNotificationsQuery request, CancellationToken cancellationToken)
        {
            var userId = _user.Id;
            var notifications = await _notificationRepository.GetNotificationsByUserIdAsync(userId.Value , request.Page, request.PageSize, cancellationToken);


            return notifications;
        }
    }


}
