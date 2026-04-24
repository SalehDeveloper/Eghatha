using Eghatha.Application.Common.Interfaces;
using Eghatha.Application.Common.Models;
using Eghatha.Application.Features.VolunteerRegisterations.Dtos;
using MediatR;

namespace Eghatha.Application.Features.VolunteerRegisterations.Queries.GetAll
{
    public class GetAllRegisterationsQueryHandler : IRequestHandler<GetAllRegisterationsQuery, PaginatedList<VolunteerRegisterationDto>>
    {
        private readonly IVolunteerRegisterationRepository _repository;

        public GetAllRegisterationsQueryHandler(IVolunteerRegisterationRepository repository)
        {
            _repository = repository;
        }

        public async Task<PaginatedList<VolunteerRegisterationDto>> Handle(GetAllRegisterationsQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetRegisterationsAsync(request.Page, request.PageSize, request.SearchTerm, request.Status, cancellationToken);
        }
    }
}
