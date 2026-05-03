using Eghatha.Application.Common.Errors;
using Eghatha.Application.Common.Interfaces;
using Eghatha.Application.Features.VolunteerRegisterations.Dtos;
using ErrorOr;
using MediatR;

namespace Eghatha.Application.Features.VolunteerRegisterations.Queries.GetById
{
    public class GetVolunteerRegisterationQueryHandlerById : IRequestHandler<GetVolunteerRegisterationQueryById, ErrorOr<VolunteerRegisterationDto>>
    {
        private readonly IVolunteerRegisterationRepository _volunteerRegisterationRepository;

        public GetVolunteerRegisterationQueryHandlerById(IVolunteerRegisterationRepository volunteerRegisterationRepository)
        {
            _volunteerRegisterationRepository = volunteerRegisterationRepository;
        }

        public async Task<ErrorOr<VolunteerRegisterationDto>> Handle(GetVolunteerRegisterationQueryById request, CancellationToken cancellationToken)
        {
            var res = await _volunteerRegisterationRepository.GetRegisterationByIdAsync(request.RegisterationId, cancellationToken);

            return res is not null ? res : ApplicationErrors.RegisterationNotFound;
        }
    }
}
