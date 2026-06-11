using MediatR;

namespace Eghatha.Application.Features.Teams.Queries.GetTeamSpecialities
{
    public sealed class GetTeamSpecialitiesQueryHandler : IRequestHandler<GetTeamSpecialitiesQuery, IReadOnlyList<TeamSpecialityResponse>>
    {
        public Task<IReadOnlyList<TeamSpecialityResponse>> Handle(GetTeamSpecialitiesQuery request, CancellationToken cancellationToken)
        {
            var specialities = Eghatha.Domain.Teams.TeamSpeciality.List
                .Select(s => new TeamSpecialityResponse(s.Value, s.Name))
                .OrderBy(x=> x.Value)
                .ToList();
                
            return Task.FromResult((IReadOnlyList<TeamSpecialityResponse>)specialities);
        }
    }


}
