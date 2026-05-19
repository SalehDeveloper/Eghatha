using Eghatha.Application.Common.Interfaces;
using Eghatha.Application.Common.Models;
using Eghatha.Application.Features.Volunteers.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Volunteers.Queries.GetTopVolunteers
{
    public class GetTopVolunteersQueryHandler : IRequestHandler<GetTopVolunteersQuery, PaginatedList<VolunteerRankingDto>>
    {
        private readonly IVolunteerRepository _volunteerRepository;

        public GetTopVolunteersQueryHandler(IVolunteerRepository volunteerRepository)
        {
            _volunteerRepository = volunteerRepository;
        }

        public async Task<PaginatedList<VolunteerRankingDto>> Handle(GetTopVolunteersQuery request, CancellationToken cancellationToken)
        {
            return await _volunteerRepository.GetTopVolunteersAsync(
           request.Page,
           request.PageSize,
           request.Province,
           request.City,
           request.Speciality,
           request.MinAverageScore,
           request.SortBy,
           request.Descending,
           cancellationToken);
        }
    }
}
