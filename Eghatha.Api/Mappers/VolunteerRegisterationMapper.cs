using Eghatha.Application.Features.VolunteerRegisterations.Dtos;
using Eghatha.Contract.VolunteerRegisterations.Responses;
using System.Runtime.CompilerServices;

namespace Eghatha.Api.Mappers
{
    public static class VolunteerRegisterationMapper
    {
        public static VolunteerRegisterationResponse ToResponse(this VolunteerRegisterationDto dto)
        {
            return new VolunteerRegisterationResponse(dto.Id, dto.VolunteerId, dto.FullName, dto.Email, dto.PhoneNumber, dto.Photo, dto.Province, dto.City,
                dto.YearsOfExperince, dto.Speciality, dto.Cv, dto.Status, dto.RequestedAt, dto.ReviewedAt, dto.RejectionReason);
        }

        public static IReadOnlyCollection<VolunteerRegisterationResponse> ToResponses(this IReadOnlyCollection<VolunteerRegisterationDto> dtos)
        {
            return dtos.Select(d => d.ToResponse()).ToList();
        }
    }
}
