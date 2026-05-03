using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Contract.Volunteers.Requests
{
    public record CreateVolunteerRequest(
        string FirstName,
        string LastName,
        string Email,
        string PhoneNumber,
        string Password,
        IFormFile Photo,
        string Speciality,
        double Latitude,
        double Longitude,
        int YearsOfExperience,
        IFormFile Cv);
    
    
}
