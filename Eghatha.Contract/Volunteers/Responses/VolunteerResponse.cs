using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Contract.Volunteers.Responses
{
    public record VolunteerResponse(Guid Id,
        string FullName,
        string Email,
        string PhoneNumber,
        string Status,
        string Speciality,
        string province,
        string city,
        int YearsOfExperience,
        double AverageScore);
    
    
}
