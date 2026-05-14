using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Common.Interfaces
{
    public interface ITeamClient
    {
        Task TeamAssignedToDisaster(Guid teamId , string refernceId ,string title,string city,string message);
    }
}
