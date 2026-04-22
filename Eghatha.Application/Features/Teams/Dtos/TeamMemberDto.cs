using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Teams.Dtos
{
    public record TeamMemberDto(
    Guid Id,
    string FullName,
    string JobTitle,
    string Status,
    bool IsLeader,
    string PhotoUrl   );


}
