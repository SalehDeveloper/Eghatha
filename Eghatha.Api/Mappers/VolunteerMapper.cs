using Eghatha.Application.Features.Teams.Dtos;
using Eghatha.Application.Features.Volunteers.Dtos;
using Eghatha.Contract.Teams.Responses;
using Eghatha.Contract.Volunteers.Responses;

namespace Eghatha.Api.Mappers
{
    public static class VolunteerMapper
    {
        public static VolunteerResponse ToResponse(this VolunteerDto dto )
        {
            return new VolunteerResponse(dto.Id, dto.FullName, dto.Email, dto.PhoneNumber, dto.Status.Name, dto.Speciality.Name, dto.province, dto.city, dto.YearsOfExperience, dto.AverageScore);
        }

        public static IReadOnlyCollection<VolunteerResponse> ToResponses(this IReadOnlyCollection<VolunteerDto> volunteers)
        {
            return volunteers.Select(t => t.ToResponse()).ToList();
        }

        public static VolunteerEquipmentResponse ToResponse(this VolunteerEquipmentDto dto)
        {
            return new VolunteerEquipmentResponse(dto.Id, dto.Name, dto.Category.Name, dto.Quantity, dto.Status.Name);
        }

        public static IReadOnlyCollection<VolunteerEquipmentResponse> ToResponses(this IReadOnlyCollection<VolunteerEquipmentDto> equipments)
        {
            return equipments.Select(t => t.ToResponse()).ToList();
        }

    }
}
