using Eghatha.Application.Common.Models;
using Eghatha.Application.Features.Disasters.Dtos;
using Eghatha.Contract.Disasters.Responses;

namespace Eghatha.Api.Mappers
{
    public static class DisasterMapper
    {
        public static CreateDisasterResponse ToResponse(this CreateDisasterDto dto)
        {
            return new CreateDisasterResponse(
                dto.DisasterId,
                dto.Status.Name,
                dto.RecommendedTeams.ToResponses(),
                dto.RecommendedVolunteers.ToResponses());
        }

        private static RecommendedTeamsResponse ToResponse(this RecommendedTeamDto dto)
        {
            return new RecommendedTeamsResponse(dto.TeamId, dto.TeamName, dto.Speciality.Name, dto.DistanceKm, dto.DurationMinutes, dto.Score, dto.IsLiveLocation);
        }

        private static RecommendedVolunteerResponse ToResponse(this RecommendedVolunteerDto dto )
        {
            return new RecommendedVolunteerResponse(dto.VolunteerId, dto.Speciality.Name, dto.DistanceKm, dto.DurationMinutes, dto.Score);
        }

        private static IReadOnlyCollection<RecommendedTeamsResponse> ToResponses(this IReadOnlyCollection<RecommendedTeamDto> dtos)
        {
            return dtos.Select(dto => dto.ToResponse()).ToList();
        }

        private static IReadOnlyCollection<RecommendedVolunteerResponse> ToResponses(this IReadOnlyCollection<RecommendedVolunteerDto> dtos)
        {
            return dtos.Select(dto => dto.ToResponse()).ToList();
        }

    }
}
