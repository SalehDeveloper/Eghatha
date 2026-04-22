using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Common.Interfaces
{
    public interface IAdminNotifier
    {
        Task NotifyLiveTeamLocationUpdated(Guid teamId , double latitude , double longitude , CancellationToken cancellationToken);
    }
}
