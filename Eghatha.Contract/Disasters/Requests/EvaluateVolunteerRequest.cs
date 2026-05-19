using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Contract.Disasters.Requests
{
    public sealed record  EvaluateVolunteerRequest(int CommitmentScore,
     int SkillScore,
     int SafetyScore,
     int TeamWorkScore,
     int InitiativeScore,
     string? Notes);
    
    
}
