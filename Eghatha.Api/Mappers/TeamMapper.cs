using Eghatha.Application.Features.Teams.Dtos;
using Eghatha.Application.Features.Teams.Queries.GetTeamDisasters;
using Eghatha.Contract.Teams.Responses;
using static Eghatha.Api.ApiEndpoints;

namespace Eghatha.Api.Mappers
{
    public static class TeamMapper
    {
        public static TeamResponse ToResponse(this TeamDto team)
        {
            return new TeamResponse(team.Id, team.Name, team.Speciality, team.Province, team.City, team.Status, team.LeaderName, team.MembersCount, team.ActiveMembersCount, team.IsReadyForMission);
        }

        public static IReadOnlyCollection<TeamResponse> ToResponses(this IReadOnlyCollection<TeamDto> teams)
        {
            return teams.Select(t => t.ToResponse()).ToList();
        }

        public static TeamMemberResponse ToResponse(this TeamMemberDto teamMember)
        {
            return new TeamMemberResponse(teamMember.Id, teamMember.FullName, teamMember.JobTitle, teamMember.Status,teamMember.IsLeader ,teamMember.PhotoUrl);
        }

        public static IReadOnlyCollection<TeamMemberResponse> ToResponses(this IReadOnlyCollection<TeamMemberDto> teamMembers)
        {
            return teamMembers.Select(t => t.ToResponse()).ToList();
        }

        public static TeamResourceResponse ToResponse(this TeamResourceDto teamResource)
        {
            return new TeamResourceResponse(teamResource.Id, teamResource.Type, teamResource.Quantity, teamResource.Status, teamResource.IsConsumable);
        }

        public static IReadOnlyCollection<TeamResourceResponse> ToResponses(this IReadOnlyCollection<TeamResourceDto> teamResources)
        {
            return teamResources.Select(t => t.ToResponse()).ToList();
        }


        public static TeamMapResponse ToResponse(this TeamMapDto team)
        {
            return new TeamMapResponse(team.Id, team.Name, team.Speciality, team.Status, team.Latitude, team.Longitude, team.IsLiveLocation, team.AssignedDisasterId);
        }

        public static List<TeamMapResponse> ToResponses(this List<TeamMapDto> teams)
        {
            return teams.Select(t => t.ToResponse()).ToList();
        }

        public static TeamDisasterResponse ToResponse(this TeamDisastersDto dto)
        {
            return new TeamDisasterResponse(dto.DisasterId, dto.Title, dto.City, dto.Province, dto.Latitude, dto.Longitude, dto.Type, dto.Status, dto.StartTime);
        }

        public static IReadOnlyCollection<TeamDisasterResponse> ToResponses(this IReadOnlyCollection<TeamDisastersDto> td)
        {
            return td.Select(t => t.ToResponse()).ToList();
        }


    }
}
