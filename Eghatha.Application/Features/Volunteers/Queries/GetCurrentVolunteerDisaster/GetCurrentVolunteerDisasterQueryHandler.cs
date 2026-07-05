using Eghatha.Application.Common.Errors;
using Eghatha.Application.Common.Interfaces;
using Eghatha.Application.Features.Volunteers.Dtos;
using ErrorOr;
using MediatR;

namespace Eghatha.Application.Features.Volunteers.Queries.GetCurrentVolunteerDisaster
{
    public sealed class GetCurrentVolunteerDisasterQueryHandler : IRequestHandler<GetCurrentVolunteerDisasterQuery, ErrorOr<VolunteerDisastersDto>>
    {
        private readonly IVolunteerRepository _volunteerRepository;

        public GetCurrentVolunteerDisasterQueryHandler(IVolunteerRepository volunteerRepository)
        {
            _volunteerRepository = volunteerRepository;
        }

        public async Task<ErrorOr<VolunteerDisastersDto>> Handle(GetCurrentVolunteerDisasterQuery request, CancellationToken cancellationToken)
        {
            var result = await _volunteerRepository.GetVolunteerDisasterAsync(request.VolunteerId, cancellationToken);


            if (result is null)
                return ApplicationErrors.NoVolunteerCurrentDisaster;

            return result;
        }        
    }




}
