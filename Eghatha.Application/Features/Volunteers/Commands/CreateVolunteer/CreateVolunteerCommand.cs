using Eghatha.Domain.Shared.ValueObjects;
using Eghatha.Domain.Volunteers;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Volunteers.Commands.CreateVolunteer
{
    public record CreateVolunteerCommand(
        string FirstName,
        string LastName,
        string Email,
        string PhoneNumber,
        string Password,
        IFormFile photo,
        VolunteerSpeciality Speciality,
        double Latitude,
        double Longitude,
        int YearsOfExperience,
        IFormFile Cv) : IRequest<ErrorOr<string>>;

}



