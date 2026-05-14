using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Common.Interfaces
{
    public interface IAdminNotifier
    {
        Task NotifyLiveTeamLocationUpdated(Guid teamId, double latitude, double longitude, CancellationToken cancellationToken);

        Task NotifyNewVolunteerRegistered(Guid referenceId, string message, string url, DateTimeOffset requestedAt, CancellationToken cancellationToken);

        Task NotifyNewDisasterReported(Guid referenceId, string message, double latitude, double longitude, string url, DateTimeOffset createdAt, CancellationToken cancellationToken);

    }
}
