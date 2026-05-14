using Eghatha.Application.Common.Models;
using Eghatha.Domain.Disasters;
using Eghatha.Domain.Volunteers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Common.Services
{
    public interface IVolunteerScoringService
    {
        double Calculate(Disaster disaster,Volunteer volunteer,RouteResult route);
    }
}
