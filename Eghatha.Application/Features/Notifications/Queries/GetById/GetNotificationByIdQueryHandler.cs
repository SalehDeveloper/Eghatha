using Eghatha.Application.Common.Errors;
using Eghatha.Application.Common.Interfaces;
using Eghatha.Application.Features.Notifications.Dtos;
using ErrorOr;
using MediatR;

namespace Eghatha.Application.Features.Notifications.Queries.GetById
{
    public sealed class GetNotificationByIdQueryHandler : IRequestHandler<GetNotificationByIdQuery, ErrorOr<NotificationDto>>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUser _user;

        public GetNotificationByIdQueryHandler(INotificationRepository notificationRepository, IUser user)
        {
            _notificationRepository = notificationRepository;
            _user = user;
        }

        public async Task<ErrorOr<NotificationDto>> Handle(GetNotificationByIdQuery request, CancellationToken cancellationToken)
        {
            var userId = _user.Id.Value;
            
            var notification = await _notificationRepository.GetNotificationByIdAsync(request.NotificationId, userId, cancellationToken);

            if (notification is null) return ApplicationErrors.NotificationNotFound;

            return notification;
        }
    }
}
