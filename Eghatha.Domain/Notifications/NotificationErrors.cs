using ErrorOr;

namespace Eghatha.Domain.Notifications
{
    public static class NotificationErrors
    {
        public static readonly Error InvalidTitle = Error.Validation(
            code: "Notification.InvalidTitle",
            description: Resources.NotificationErrors.Notification_InvalidTitle);


        public static readonly Error InvalidMessage = Error.Validation(
            code: "Notification.InvalidMessage",
            description: Resources.NotificationErrors.Notification_InvalidMessage);


        public static readonly Error InvalidUrl = Error.Validation(
            code: "Notification.InvalidUrl",
            description: Resources.NotificationErrors.Notification__InvalidUrl);




    }
}
