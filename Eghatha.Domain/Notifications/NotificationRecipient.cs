using Eghatha.Domain.Abstractions;
using Eghatha.Domain.Shared.Errors;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Domain.Notifications
{
    public class NotificationRecipient : Entity
    {


        public Guid NotificationId { get; private set; }

        public Guid UserId { get; private set; }

        public bool IsRead { get; private set; }

        public DateTimeOffset? ReadAt { get; private set; }

        public Notification Notification { get; private set; }

        private NotificationRecipient(Guid id, Guid notificationId, Guid userId, bool isRead)
            : base(id)
        {
            NotificationId = notificationId;
            UserId = userId;
            IsRead = isRead;
        }

        public static ErrorOr<NotificationRecipient> Create(Guid notificationId, Guid userId)
        {

            if (notificationId == Guid.Empty)
                return DomainErrors.IdMustBeProvided("Notification");


            if (userId == Guid.Empty)
                return DomainErrors.IdMustBeProvided("User");


            return new NotificationRecipient(Guid.NewGuid(), notificationId, userId, false);


        }


        public void MarkAsRead(DateTimeOffset readAt)
        {

            if (IsRead) return;

            IsRead = true;
            ReadAt = readAt;

            
        }
    }
}