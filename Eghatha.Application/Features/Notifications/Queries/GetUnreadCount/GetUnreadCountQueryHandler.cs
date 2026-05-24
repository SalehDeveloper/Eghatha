using Eghatha.Application.Common.Interfaces;
using MediatR;

namespace Eghatha.Application.Features.Notifications.Queries.GetUnreadCount
{
    public sealed class GetUnreadCountQueryHandler : IRequestHandler<GetUnreadCountQuery, int>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUser _user;

        public GetUnreadCountQueryHandler(INotificationRepository notificationRepository, IUser user)
        {
            _notificationRepository = notificationRepository;
            _user = user;
        }
        public async Task<int> Handle(GetUnreadCountQuery request, CancellationToken cancellationToken)
        {
            var userId = _user.Id.Value;
            return await _notificationRepository.GetUnreadCountAsync( userId, cancellationToken);
        }
    }
}
