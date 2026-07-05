using Eghatha.Application.Features.Teams.Dtos;
using Eghatha.Application.Features.Volunteers.Dtos;
using Eghatha.Contract.Teams.Responses;
using Eghatha.Contract.Volunteers.Responses;

namespace Eghatha.Api.Mappers
{
    public static class VolunteerMapper
    {
        public static VolunteerResponse ToResponse(this VolunteerDto dto)
        {
            return new VolunteerResponse(dto.Id, dto.FullName, dto.Email, dto.PhoneNumber, dto.Status, dto.Speciality, dto.province, dto.city, dto.YearsOfExperience, dto.AverageScore);
        }

        public static IReadOnlyCollection<VolunteerResponse> ToResponses(this IReadOnlyCollection<VolunteerDto> volunteers)
        {
            return volunteers.Select(t => t.ToResponse()).ToList();
        }

        public static VolunteerEquipmentResponse ToResponse(this VolunteerEquipmentDto dto)
        {
            return new VolunteerEquipmentResponse(dto.Id, dto.Name, dto.Category, dto.Quantity, dto.Status);
        }

        public static IReadOnlyCollection<VolunteerEquipmentResponse> ToResponses(this IReadOnlyCollection<VolunteerEquipmentDto> equipments)
        {
            return equipments.Select(t => t.ToResponse()).ToList();
        }

        public static volunteerRankingResponse ToResponse(this VolunteerRankingDto dto)
        {
            return new volunteerRankingResponse(dto.VolunteerId, dto.FullName, dto.Speciality, dto.Province, dto.City, dto.TotalMissions, dto.TotalScore, dto.AverageScore, dto.Rank);
        }

        public static IReadOnlyCollection<volunteerRankingResponse> ToResponses(this IReadOnlyCollection<VolunteerRankingDto> volunteers)
        {
            return volunteers.Select(t => t.ToResponse()).ToList();
        }


        public static VolunteerDisasterResponse ToResponse(this VolunteerDisastersDto dto)
        {
            return new VolunteerDisasterResponse(dto.DisasterId, dto.Title, dto.City, dto.Province, dto.Latitude, dto.Longitude, dto.Type, dto.Status, dto.StartTime);
        }
    }
}
