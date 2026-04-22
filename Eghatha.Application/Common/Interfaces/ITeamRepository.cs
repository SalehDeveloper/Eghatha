using Eghatha.Application.Common.Models;
using Eghatha.Application.Features.Teams.Dtos;
using Eghatha.Domain.Abstractions;
using Eghatha.Domain.Teams;
using Eghatha.Domain.Teams.Resources;
using Eghatha.Domain.Teams.TeamMembers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Common.Interfaces
{
    public interface ITeamRepository : IBaseRepository<Team>
    {
        Task<Team?> GetTeamForAUserAsync(Guid userId, CancellationToken cancellationToken);

        Task<Team?> GetTeamByIdWithMembersAsync(Guid id, CancellationToken cancellationToken);

       Task AddTeamMemberAsync(TeamMember member, CancellationToken cancellationToken);

        Task<Team?> GetTeamByIdWithResourcesAsync(Guid id, CancellationToken cancellationToken);

        Task AddTeamResourceAsync(Resource resource, CancellationToken cancellationToken);

        Task<PaginatedList<TeamDto>> GetTeamsAsync(
           int page,
           int pageSize,
           string? searchTerm,
           TeamStatus? status,
           TeamSpeciality? speciality,
           string? province,
           string? city,
           CancellationToken cancellationToken);

        Task<TeamDto?> GetTeamOverviewAsync(
    Guid teamId,
    CancellationToken cancellationToken);

        Task<PaginatedList<TeamMemberDto>> GetTeamMembersAsync(
    Guid teamId,
    int page,
    int pageSize,
    string? searchTerm,
    TeamMemberStatus? status,
    CancellationToken cancellationToken);

        Task<PaginatedList<TeamResourceDto>> GetTeamResourcesAsync(
   Guid teamId,
   int page,
   int pageSize,
   ResourceType? type,
   CancellationToken cancellationToken);


    }
}
