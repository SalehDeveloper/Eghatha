using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Contract.VolunteerRegisterations.Responses
{
    public record VolunteerRegisterationResponse(Guid Id,
    Guid VolunteerId,
    string FullName,
    string Email,
    string PhoneNumber,
    string Photo,
    double Latitude,
    double Longitude,
    int YearsOfExperince,
    string Speciality,
    string Cv,
    string Status,
    DateTimeOffset RequestedAt,
    DateTimeOffset? ReviewedAt,
    string? RejectionReason
        );
    
    
}
