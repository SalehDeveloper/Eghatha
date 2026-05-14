using Eghatha.Application.Common.Models;
using Eghatha.Domain.Disasters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Disasters.Dtos
{
    public sealed record CreateDisasterDto(
    Guid DisasterId,
    DisasterStatus Status,
    IReadOnlyList<RecommendedTeamDto> RecommendedTeams,
    IReadOnlyList<RecommendedVolunteerDto> RecommendedVolunteers);
    
    
}
