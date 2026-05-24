using Eghatha.Application.Common.Interfaces;
using Eghatha.Domain.Abstractions;
using ErrorOr;
using MediatR;

namespace Eghatha.Application.Features.Notifications.Commands.MarkAllAsRead
{
    public sealed class MarkAllNotificationsAsReadCommandHandler : IRequestHandler<MarkAllNotificationsAsReadCommand, ErrorOr<Updated>>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUser _user;
        private readonly TimeProvider _timeProvider;
        private readonly IUnitOfWork _unitOfWork;



        public MarkAllNotificationsAsReadCommandHandler(INotificationRepository notificationRepository, IUser user, TimeProvider timeProvider, IUnitOfWork unitOfWork)
        {
            _notificationRepository = notificationRepository;
            _user = user;
            _timeProvider = timeProvider;
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<Updated>> Handle(MarkAllNotificationsAsReadCommand request, CancellationToken cancellationToken)
        {
            var userId = _user.Id.Value;

            var result = await _notificationRepository.GetNotificationRecipientsByUserIdAsync( userId, cancellationToken);
           
            if (result is null || !result.Any())
            {
                return Error.NotFound("Notifications.NotFound", "No notifications found for the user.");
            }

            foreach (var recipient in result)
            {
                recipient.MarkAsRead(_timeProvider.GetUtcNow());
            }

            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result.Updated;

        }
    }



}
