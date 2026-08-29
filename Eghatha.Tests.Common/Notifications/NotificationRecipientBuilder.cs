using Eghatha.Domain.Notifications;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Tests.Common.Notifications
{
    /// <summary>
    /// Fluent builder that produces a valid <see cref="NotificationRecipient"/>
    /// by default. Use the With* methods to override individual fields when a
    /// test needs to exercise a specific validation branch.
    /// </summary>
    public sealed class NotificationRecipientBuilder
    {
        private Guid _notificationId = Guid.NewGuid();
        private Guid _userId = Guid.NewGuid();

        public static NotificationRecipientBuilder Valid() => new();

        public NotificationRecipientBuilder WithNotificationId(Guid notificationId)
        {
            _notificationId = notificationId;
            return this;
        }

        public NotificationRecipientBuilder WithUserId(Guid userId)
        {
            _userId = userId;
            return this;
        }

        public ErrorOr<NotificationRecipient> Build() =>
            NotificationRecipient.Create(_notificationId, _userId);

        /// <summary>
        /// Builds and unwraps the result. Only use this in tests where the
        /// input is known-valid (arrange phase) — never in the tests that
        /// are actually asserting on Create's validation behavior.
        /// </summary>
        public NotificationRecipient BuildValid() => Build().Value;
    }
}
