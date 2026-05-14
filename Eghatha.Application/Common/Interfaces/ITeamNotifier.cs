using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Common.Interfaces
{
    public interface ITeamNotifier
    {
        Task NotifyTeamAssignedToDisaster(
       Guid teamId,
       string refernceId,
       string title,
       string city,
       string message,
       CancellationToken cancellationToken);
    }
}
