using Eghatha.Application.Common.Interfaces;
using Eghatha.Application.Common.Models;
using Eghatha.Application.Features.Volunteers.Dtos;
using MediatR;

namespace Eghatha.Application.Features.Volunteers.Queries.GetEquipments
{
    public class GetVolunteerEquipmentsQueryHandler
    : IRequestHandler<GetVolunteerEquipmentsQuery, PaginatedList<VolunteerEquipmentDto>>
    {
        private readonly IVolunteerRepository _repo;

        public GetVolunteerEquipmentsQueryHandler(
            IVolunteerRepository repo)
        {
            _repo = repo;
        }

        public async Task<PaginatedList<VolunteerEquipmentDto>> Handle(
            GetVolunteerEquipmentsQuery request,
            CancellationToken cancellationToken)
        {
            return await _repo.GetVolunteerEquipmentsAsync(
                request.VolunteerId,
                request.Page,
                request.PageSize,
                request.Category,
                cancellationToken);
        }
    }

}
