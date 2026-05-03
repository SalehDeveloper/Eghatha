using Eghatha.Application.Common.Interfaces;
using Eghatha.Application.Common.Models;
using Eghatha.Application.Features.Volunteers.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Volunteers.Queries.GetAll
{
    public class GetAllVolunteersQueryHandler : IRequestHandler<GetAllVolunteersQuery, PaginatedList<VolunteerDto>>
    {
        private readonly IVolunteerRepository _repository;

        public GetAllVolunteersQueryHandler(IVolunteerRepository repository)
        {
            _repository = repository;
        }

        public async Task<PaginatedList<VolunteerDto>> Handle(GetAllVolunteersQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetVolunteersAsync(request.Page, request.PageSize, request.SearchTerm, request.Status, request.Speciality, request.Province, request.City , cancellationToken);
        }
    }
}
