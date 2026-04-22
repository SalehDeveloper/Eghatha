using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Contract.Teams.Responses
{
    public record TeamResponse(
    Guid Id,
    string Name,
    string Speciality,
    string Province,
    string City,
    string Status,
    string? LeaderName,
    int MembersCount,
    int ActiveMembersCount,
    bool IsReadyForMission);
    
    
}
