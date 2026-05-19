using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Contract.Volunteers.Requests
{
    public sealed record GetTopVolunteersFilter(string? Province,
     string? City,
     string? Speciality,
     double? MinAverageScore,
     string SortBy="AverageScore",
     bool Descending = true);
    
    
}
