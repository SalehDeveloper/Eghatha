using Eghatha.Application.Common.Interfaces;
using Eghatha.Application.Features.Volunteers.Dtos;
using MediatR;

namespace Eghatha.Application.Features.Volunteers.Queries.GetById
{
    public class GetVolunteerByIdQueryHandler : IRequestHandler<GetVolunteerByIdQuery, VolunteerDto>
    {
        private readonly IVolunteerRepository _volunteerRepository;

        public GetVolunteerByIdQueryHandler(IVolunteerRepository volunteerRepository)
        {
            _volunteerRepository = volunteerRepository;
        }

        public async Task<VolunteerDto> Handle(GetVolunteerByIdQuery request, CancellationToken cancellationToken)
        {
            return await _volunteerRepository.GetVolunteerDetailsByIdAsync(request.Id, cancellationToken);
        }
    }
}
