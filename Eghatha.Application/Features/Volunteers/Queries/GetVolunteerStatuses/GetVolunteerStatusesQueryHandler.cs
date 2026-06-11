using Eghatha.Domain.Volunteers;
using MediatR;

namespace Eghatha.Application.Features.Volunteers.Queries.GetVolunteerStatuses
{
    public sealed class GetVolunteerStatusesQueryHandler
    : IRequestHandler<GetVolunteerStatusesQuery, IReadOnlyList<VolunteerStatusResponse>>
    {
        public Task<IReadOnlyList<VolunteerStatusResponse>> Handle(
            GetVolunteerStatusesQuery request,
            CancellationToken cancellationToken)
        {
            var result = VolunteerStatus.List
                .Select(x => new VolunteerStatusResponse(x.Value, x.Name))
                 .OrderBy(x => x.Value)
                .ToList();

            return Task.FromResult<IReadOnlyList<VolunteerStatusResponse>>(result);
        }
    }
}
