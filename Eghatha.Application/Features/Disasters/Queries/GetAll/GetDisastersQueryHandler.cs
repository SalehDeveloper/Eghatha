using Eghatha.Application.Common.Interfaces;
using Eghatha.Application.Common.Models;
using Eghatha.Application.Features.Disasters.Dtos;
using MediatR;

namespace Eghatha.Application.Features.Disasters.Queries.GetAll
{
    public class GetDisastersQueryHandler
    : IRequestHandler<GetDisastersQuery, PaginatedList<DisasterDto>>
    {
        private readonly IDisasterRepository _repo;

        public GetDisastersQueryHandler(IDisasterRepository repo)
        {
            _repo = repo;
        }

        public async Task<PaginatedList<DisasterDto>> Handle(
            GetDisastersQuery request,
            CancellationToken cancellationToken)
        {
            return await _repo.GetDisastersAsync(
                request.Page,
                request.PageSize,
                request.City,
                request.Province,
                request.Type,
                request.Status,
                request.From,
                request.To,
                cancellationToken);
        }
    }
}
