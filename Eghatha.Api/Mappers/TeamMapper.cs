using Eghatha.Application.Features.Teams.Dtos;
using Eghatha.Contract.Teams.Responses;

namespace Eghatha.Api.Mappers
{
    public static class TeamMapper
    {
        public static TeamResponse ToResponse(this TeamDto team)
        {
            return new TeamResponse(team.Id, team.Name, team.Speciality.Name, team.Province, team.City, team.Status.Name, team.LeaderName, team.MembersCount, team.ActiveMembersCount, team.IsReadyForMission);
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

    }
}
