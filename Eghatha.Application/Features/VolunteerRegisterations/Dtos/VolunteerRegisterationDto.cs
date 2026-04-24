using Eghatha.Domain.Shared.ValueObjects;
using Eghatha.Domain.VolunteerRegisterations;
using Eghatha.Domain.Volunteers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.VolunteerRegisterations.Dtos
{
    public record VolunteerRegisterationDto(Guid Id,
    Guid VolunteerId,
    string FullName,
    string Email,
    string PhoneNumber ,
    string Photo,
    double Latitude,
    double Longitude,
    int YearsOfExperince , 
    VolunteerSpeciality Speciality ,
    string Cv ,
    RegisterationStatus Status,
    DateTimeOffset RequestedAt,
    DateTimeOffset? ReviewedAt,
    string? RejectionReason);
    
}
