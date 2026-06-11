using Eghatha.Application.Common.Models;
using Eghatha.Application.Features.Teams.Dtos;
using Eghatha.Application.Features.Teams.Queries.GetTeamDisasters;
using Eghatha.Application.Features.Teams.Queries.GetTeamMemberInfo;
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
        Task<PaginatedList<TeamDto>> GetTeamsAsync(int page, int pageSize, string? searchTerm, string? status, string? speciality, string? province, string? city, CancellationToken cancellationToken);
        Task<TeamDto?> GetTeamOverviewAsync( Guid teamId, CancellationToken cancellationToken);

        Task<PaginatedList<TeamResourceDto>> GetTeamResourcesAsync(Guid teamId, int page, int pageSize, string? type, CancellationToken cancellationToken);
        Task<IReadOnlyList<Team>> GetAvailableTeamsAsync(IReadOnlyList<TeamSpeciality> specialities, CancellationToken cancellationToken);
        Task<List<Team>> GetTeamsByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken);

        Task<Guid?> GetTeamLeaderByUserId(Guid userId, CancellationToken cancellationToken);

        Task<PaginatedList<TeamMemberDto>> GetTeamMembersAsync(Guid teamId, int page, int pageSize, string? searchTerm, string? status, CancellationToken cancellationToken);

        Task<TeamMemberInfo> GetCurrentTeamMemberInfo(Guid userId, CancellationToken cancellationToken);

        Task<List<TeamMapDto>> GetTeamsOnMapAsync(CancellationToken cancellationToken);

        Task<PaginatedList<TeamDisastersDto>> GetTeamDisastersAsync(Guid teamId, int page, int pageSize, CancellationToken cancellationToken);
        Task<TeamDisastersDto> GetTeamDisasterAsync(Guid teamId, CancellationToken cancellationToken);


    }
}
