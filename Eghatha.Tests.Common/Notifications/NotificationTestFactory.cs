using Eghatha.Domain.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Tests.Common.Notifications
{
    /// <summary>
    /// Produces Notification aggregates in commonly-needed shapes for tests.
    /// Every mutation goes through the aggregate's own public methods (never
    /// reflection/internal state hacks).
    /// </summary>
    public static class NotificationTestFactory
    {
        public static Notification CreateValid() => NotificationBuilder.Valid().BuildValid();

        /// <summary>
        /// A valid notification with a single recipient already attached, for
        /// tests exercising recipient behavior without repeating the
        /// AddRecipient arrange step in every test.
        /// </summary>
        public static Notification CreateWithRecipient(out NotificationRecipient recipient, Guid? userId = null)
        {
            var notification = CreateValid();
            notification.AddRecipient(userId ?? Guid.NewGuid());
            recipient = notification.Recipients.Single();
            return notification;
        }
    }
}
