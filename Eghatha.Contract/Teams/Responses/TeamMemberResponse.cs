using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Contract.Teams.Responses
{
    public record TeamMemberResponse(Guid Id,
    string FullName,
    string JobTitle,
    string Status,
    bool IsLeader,
    string PhotoUrl);
    
}
