using Eghatha.Domain.Abstractions;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Domain.Notifications
{
    public class Notification : AuditableEntity
    {
        private Notification(Guid id , string title, string message, string url, NotificationType type)
            :base(id)
        {
            Title = title;
            Message = message;
            Url = url;
            Type = type;
        }

        public string Title { get; private  set; }

        public string Message { get; private set; }

        public string Url { get; private set; }

        public NotificationType Type { get; private set; }

        private readonly List<NotificationRecipient> _recipients = [];

        public IReadOnlyCollection<NotificationRecipient>  Recipients => _recipients.AsReadOnly();


        public static ErrorOr<Notification> Create(string title, string message, string url, NotificationType type)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return NotificationErrors.InvalidTitle;
            }
            if (string.IsNullOrWhiteSpace(message))
            {
                return NotificationErrors.InvalidMessage;
            }
            if (string.IsNullOrWhiteSpace(url))
            {
                return NotificationErrors.InvalidUrl;
            }
            var notification = new Notification(  Guid.NewGuid(), title, message, url, type);

            return notification;
        }

        public void AddRecipient(Guid userId)
        {
            _recipients.Add(
                 NotificationRecipient.Create(
           
                    Id,
                    userId).Value);
        }

    }
}
