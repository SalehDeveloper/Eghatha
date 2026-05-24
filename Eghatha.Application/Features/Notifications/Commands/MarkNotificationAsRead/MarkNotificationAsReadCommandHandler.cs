using Eghatha.Application.Common.Errors;
using Eghatha.Application.Common.Interfaces;
using Eghatha.Domain.Abstractions;
using ErrorOr;
using MediatR;

namespace Eghatha.Application.Features.Notifications.Commands.MarkNotificationAsRead
{
    public sealed class MarkNotificationAsReadCommandHandler : IRequestHandler<MarkNotificationAsReadCommand, ErrorOr<Updated>>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUser _user;
        private readonly TimeProvider _timeProvider;
        private readonly IUnitOfWork _unitOfWork;



        public MarkNotificationAsReadCommandHandler(INotificationRepository notificationRepository, IUser user, TimeProvider timeProvider, IUnitOfWork unitOfWork)
        {
            _notificationRepository = notificationRepository;
            _user = user;
            _timeProvider = timeProvider;
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<Updated>> Handle(MarkNotificationAsReadCommand request, CancellationToken cancellationToken)
        {

            var userId = _user.Id.Value;

            var notificationRecipient = await _notificationRepository.GetNotificationRecipientAsync(request.NotificationId, userId, cancellationToken);

            if (notificationRecipient is null)
                return ApplicationErrors.NotificationNotFound;

            notificationRecipient.MarkAsRead(_timeProvider.GetUtcNow());

            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result.Updated;
        }
    }
}
