using Eghatha.Domain.Volunteers;
using MediatR;

namespace Eghatha.Application.Features.Volunteers.Queries.GetVolunteerSpecialities
{
    public sealed class GetVolunteerSpecialitiesQueryHandler
    : IRequestHandler<GetVolunteerSpecialitiesQuery, IReadOnlyList<VolunteerSpecialityResponse>>
    {
        public Task<IReadOnlyList<VolunteerSpecialityResponse>> Handle(
            GetVolunteerSpecialitiesQuery request,
            CancellationToken cancellationToken)
        {
            var result = VolunteerSpeciality.List
                .Select(x => new VolunteerSpecialityResponse(x.Value, x.Name))
                .ToList();

            return Task.FromResult<IReadOnlyList<VolunteerSpecialityResponse>>(result);
        }
    }
}
