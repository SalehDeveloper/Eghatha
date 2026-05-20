using Eghatha.Domain.VolunteerRegisterations;
using MediatR;

namespace Eghatha.Application.Features.VolunteerRegisterations.Queries.GetRegistrationStatuses
{
    public sealed class GetRegistrationStatusesQueryHandler
    : IRequestHandler<GetRegistrationStatusesQuery, IReadOnlyList<RegistrationStatusResponse>>
    {
        public Task<IReadOnlyList<RegistrationStatusResponse>> Handle(
            GetRegistrationStatusesQuery request,
            CancellationToken cancellationToken)
        {
            var result = RegisterationStatus.List
                .Select(x => new RegistrationStatusResponse(x.Value, x.Name))
                .ToList();

            return Task.FromResult<IReadOnlyList<RegistrationStatusResponse>>(result);
        }
    }
}
