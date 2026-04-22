using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Contract.Teams.Requests
{
    public record GetTeamsFilter(string? SearchTerm, string? Status, string? Speciality, string? Province, string? City);
    
    
}
