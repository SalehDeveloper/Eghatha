using Eghatha.Application.Common.Models;
using Eghatha.Application.Features.Disasters.Dtos;
using Eghatha.Application.Features.Disasters.Queries.GetDisasterVolunteers;
using Eghatha.Contract.Disasters.Responses;
using Eghatha.Domain.Disasters.AffectedPersons;
using MimeKit.Cryptography;

namespace Eghatha.Api.Mappers
{
    public static class DisasterMapper
    {

        public static SpamCheckResponse ToResponse(this SpamCheckResultDto dto) =>
    new(dto.IsSpam, dto.MatchedDisasterId, dto.Confidence, dto.Reasoning);
        public static CreateDisasterResponse ToResponse(this CreateDisasterDto dto)
        {
            return new CreateDisasterResponse(
                dto.DisasterId,
                dto.Status.Name,
                dto.RecommendedTeams.ToResponses(),
                dto.RecommendedVolunteers.ToResponses());
        }

        public static RecommendedTeamsResponse ToResponse(this RecommendedTeamDto dto)
        {
            return new RecommendedTeamsResponse(dto.TeamId, dto.TeamName, dto.Speciality.Name, dto.Province, dto.City, dto.DistanceKm, dto.DurationMinutes, dto.Score, dto.IsLiveLocation);
        }

        public static RecommendedVolunteerResponse ToResponse(this RecommendedVolunteerDto dto)
        {
            return new RecommendedVolunteerResponse(dto.VolunteerId, dto.Speciality.Name, dto.DistanceKm, dto.DurationMinutes, dto.Score);
        }

        public static IReadOnlyCollection<RecommendedTeamsResponse> ToResponses(this IReadOnlyCollection<RecommendedTeamDto> dtos)
        {
            return dtos.Select(dto => dto.ToResponse()).ToList();
        }

        public static IReadOnlyCollection<RecommendedVolunteerResponse> ToResponses(this IReadOnlyCollection<RecommendedVolunteerDto> dtos)
        {
            return dtos.Select(dto => dto.ToResponse()).ToList();
        }

        private static AffectedPersonDto ToDto(this Contract.Disasters.Requests.AffectedPersonDto response)
        {


            return new AffectedPersonDto(response.Name, response.Age, response.Phone, response.Status, response.Notes);
        }

        public static List<AffectedPersonDto> ToDtos(this IReadOnlyCollection<Contract.Disasters.Requests.AffectedPersonDto> responses)
        {
            return responses.Select(response => response.ToDto()).ToList();
        }

        public static DisasterResponse ToResponse(this DisasterDto dto)
        {
            return new DisasterResponse(dto.Id, dto.Title, dto.City, dto.Province, dto.Latitude, dto.Longitude, dto.Type, dto.Status, dto.StartTime);

        }

        public static IReadOnlyCollection<DisasterResponse> ToResponses(this IReadOnlyCollection<DisasterDto> dtos)
        {
            return dtos.Select(dto => dto.ToResponse()).ToList();

        }

        public static ReporterResponse ToResponse(this ReporterDto dto)
        {
            return new ReporterResponse(dto.Name, dto.Phone);
        }

        public static DisasterTeamResponse ToResponse(this TeamDto dto)
        {
            return new DisasterTeamResponse(dto.TeamId, dto.TeamName);
        }

        public static ResourceResponse ToResponse(this ResourceDto dto)
        {
            return new ResourceResponse(dto.Id, dto.ResourceType, dto.Sent, dto.Consumed, dto.Returned, dto.Damaged, dto.Notes);
        }
        public static ReportResponse ToResponse(this ReportDto dto)
        {
            return new ReportResponse(dto.Id, dto.Summary, dto.PdfUrl, dto.IssuedAt);
        }

        public static AffectedPersonResponse ToResponse(this AffectedPersonDto dto)
        {
            return new AffectedPersonResponse(dto.Name, dto.Age, dto.Phone, dto.Status, dto.Notes);
        }

        public static DisasterDetailsResponse ToResponse(this DisasterDetailsDto dto)
        {
            return new DisasterDetailsResponse(dto.Id,
                dto.Title,
                dto.Description,
                dto.City,
                dto.Province,
                dto.Type,
                dto.Status,
                dto.Latitude,
                dto.Longitude,
                dto.StartTime,
                dto.EndTime,
                dto.Reporter.ToResponse(),
                dto.Teams.Select(t => t.ToResponse()).ToList(),
                dto.Resources.Select(r => r.ToResponse()).ToList(),
                dto.AffectedPeople.Select(p => p.ToResponse()).ToList(),
                dto.Report?.ToResponse()
                );
        }


        public static DisasterTimeLineResponse ToResponse(this DisasterTimelineDto dto)
        {
            return new DisasterTimeLineResponse(dto.Id, dto.EventType, dto.Description, dto.OccurredAt);
        }

        public static IReadOnlyCollection<DisasterTimeLineResponse> ToResponses(this IReadOnlyCollection<DisasterTimelineDto> dtos)
        {
            return dtos.Select(dto => dto.ToResponse()).ToList();
        }


        public static DisasterVolunteerResponse ToResponse(this DisasterVolunteerDto dto)
        {
            return new DisasterVolunteerResponse(dto.Id, dto.Name, dto.Email, dto.PhoneNumber, dto.PhotoUrl, dto.Status);
        }

        public static IReadOnlyCollection<DisasterVolunteerResponse> ToResponses(this IReadOnlyCollection<DisasterVolunteerDto> dtos)
        {
            return dtos.Select(dto => dto.ToResponse()).ToList();
        }

    }
}
