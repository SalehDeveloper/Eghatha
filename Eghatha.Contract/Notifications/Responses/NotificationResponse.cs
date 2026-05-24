using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Contract.Notifications.Responses
{
    public sealed record NotificationResponse(Guid Id, string Title, string Message, string Url, string Type, bool IsRead, DateTimeOffset CreatedAt);
    
    
}
