using Eghatha.Domain.Teams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Teams.Dtos
{
    public record TeamDto(Guid Id,
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
