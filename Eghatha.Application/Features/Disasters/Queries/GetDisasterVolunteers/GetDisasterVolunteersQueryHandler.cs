using Eghatha.Application.Common.Interfaces;
using Eghatha.Application.Common.Models;
using MediatR;

namespace Eghatha.Application.Features.Disasters.Queries.GetDisasterVolunteers
{
    public sealed class GetDisasterVolunteersQueryHandler : IRequestHandler<GetDisasterVolunteersQuery, PaginatedList<DisasterVolunteerDto>>
    {
        private readonly IDisasterRepository _disasterRepository;
        public GetDisasterVolunteersQueryHandler(IDisasterRepository disasterRepository)
        {
            _disasterRepository = disasterRepository;
        }
        public async Task<PaginatedList<DisasterVolunteerDto>> Handle(GetDisasterVolunteersQuery request, CancellationToken cancellationToken)
        {
            var volunteers = await _disasterRepository.GetDisasterVolunteersAsync(request.DisasterId , request.Page , request.PageSize , cancellationToken);


            return volunteers;
        }

    }
}
