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
    }
}
