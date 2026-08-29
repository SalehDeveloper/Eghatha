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
    /// Fluent builder that produces a valid <see cref="Notification"/> by default.
    /// Use the With* methods to override individual fields when a test needs
    /// to exercise a specific validation branch (e.g. WithTitle(null)).
    ///
    /// NOTE: Notification.Create generates its own Id internally (it isn't
    /// accepted as a parameter), so there's no WithId here.
    /// </summary>
    public sealed class NotificationBuilder
    {
        private string _title = "Test Notification";
        private string _message = "Test notification message";
        private string _url = "https://example.com/notifications/test";
        private NotificationType _type = NotificationType.DisasterReported;

        public static NotificationBuilder Valid() => new();

        public NotificationBuilder WithTitle(string title)
        {
            _title = title;
            return this;
        }

        public NotificationBuilder WithMessage(string message)
        {
            _message = message;
            return this;
        }

        public NotificationBuilder WithUrl(string url)
        {
            _url = url;
            return this;
        }

        public NotificationBuilder WithType(NotificationType type)
        {
            _type = type;
            return this;
        }

        public ErrorOr<Notification> Build() =>
            Notification.Create(_title, _message, _url, _type);

        /// <summary>
        /// Builds and unwraps the result. Only use this in tests where the
        /// input is known-valid (arrange phase) — never in the tests that
        /// are actually asserting on Create's validation behavior.
        /// </summary>
        public Notification BuildValid() => Build().Value;
    }
}
