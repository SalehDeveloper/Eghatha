using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Contract.Volunteers.Requests
{
    public record GetVolunteersFilter(string? SearchTerm, string? Status, string? Speciality, string? Province, string? City);
}
