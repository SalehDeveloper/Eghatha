using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Domain.Notifications
{
    public static class NotificationErrors
    { 
        public static readonly Error InvalidTitle = Error.Validation(
            code:"Notification.InvalidTitle",
            description:"The notification title is invalid.");


        public static readonly Error InvalidMessage = Error.Validation(
            code: "Notification.InvalidMessage",
            description: "The notification message is invalid.");


        public static readonly Error InvalidUrl = Error.Validation(
            code: "Notification.InvalidUrl",
            description: "The notification url is invalid.");




    }
}
