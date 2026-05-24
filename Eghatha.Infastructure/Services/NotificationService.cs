using Eghatha.Application.Common.Models;
using Eghatha.Application.Common.Services;
using Eghatha.Domain.Notifications;
using Eghatha.Infastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Infastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly AppDbContext _dbContext;

        public NotificationService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SendAsync(NotificationRequest request, CancellationToken cancellationToken = default)
        {
            var notification = Notification.Create(request.Title, request.Message, request.Url, request.Type);

            foreach (var userId in request.UserIds)
            {
                notification.Value.AddRecipient(userId);
            }

            await _dbContext.Set<Notification>().AddAsync(notification.Value, cancellationToken);

            await _dbContext.Set<NotificationRecipient>().AddRangeAsync(notification.Value.Recipients, cancellationToken);

            return;
        }
    }
}
