using Eghatha.Application.Common.Authentication;
using Eghatha.Application.Common.Models;
using Eghatha.Application.Common.Services;
using Eghatha.Domain.Abstractions;
using Eghatha.Domain.Disaster;
using Eghatha.Domain.Notifications;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Disasters.EventHandlers
{
    public sealed class DisasterCreatedNotificationHandler : INotificationHandler<DisasterCreated>
    {
        private readonly INotificationService _notificationService;
        private readonly ILogger<DisasterCreatedNotificationHandler> _logger;
        private readonly IIdentityCacheService _identityCacheService;

        public DisasterCreatedNotificationHandler(INotificationService notificationService, ILogger<DisasterCreatedNotificationHandler> logger, IIdentityCacheService identityCacheService)
        {
            _notificationService = notificationService;
            _logger = logger;
            _identityCacheService = identityCacheService;
        }

        public async Task Handle(DisasterCreated notification, CancellationToken cancellationToken)
        {
            var sw1 = Stopwatch.StartNew();

            var adminIds = await _identityCacheService.GetAdminIdsAsync(cancellationToken);
            

            _logger.LogInformation("AddAsync: {ms}", sw1.ElapsedMilliseconds);

            var request = new NotificationRequest
            {
                Title = "New Disaster Reported",
                Message =
                    $"A new disaster of type {notification.Type} has been reported in {notification.Province}, {notification.City}",
                Url = $"/disasters/{notification.Id}",
                UserIds = adminIds,
                Type = NotificationType.DisasterReported
            };

            var sw2 = Stopwatch.StartNew();

            await _notificationService.SendAsync(
                request,
                cancellationToken);

            _logger.LogInformation("AddAsync: {ms}", sw2.ElapsedMilliseconds);
        }
    }
}
