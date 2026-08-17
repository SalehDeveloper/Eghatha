using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Common.Interfaces
{
    public interface IAdminTrackingClient
    {
        Task TeamLiveLocationUpdated(Guid teamId , double latitude , double longitude);

        Task NewVolunteerRegisterd(Guid referenceId, string message,  string url , DateTimeOffset requestedAt);

        Task NewDisasterReported(Guid referenceId, string message, double latitude ,double Longitude ,   string url, DateTimeOffset createdAt);

        Task DisasterResolved(Guid disasterId, DateTimeOffset resolvedAt);

        Task TeamStatusUpdated(Guid teamId, string status);

        Task DisasterClosed(Guid disasterId);

        Task TeamAssignedToDisaster(Guid teamId, Guid disasterId);
    }
}
