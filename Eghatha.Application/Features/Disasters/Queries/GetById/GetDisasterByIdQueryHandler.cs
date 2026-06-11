using Eghatha.Application.Common.Interfaces;
using Eghatha.Application.Features.Disasters.Dtos;
using MediatR;

namespace Eghatha.Application.Features.Disasters.Queries.GetById
{
    public class GetDisasterByIdQueryHandler
    : IRequestHandler<GetDisasterByIdQuery, DisasterDetailsDto>
    {
        private readonly IDisasterRepository _repo;
        private readonly ITeamRepository _teamRepository;

        public GetDisasterByIdQueryHandler(
            IDisasterRepository repo,
            ITeamRepository teamRepository)
        {
            _repo = repo;
            _teamRepository = teamRepository;
        }

        public async Task<DisasterDetailsDto> Handle(
            GetDisasterByIdQuery request,
            CancellationToken cancellationToken)
        {
            var disaster = await _repo.GetByIdWithAllDetailsAsync(
                request.DisasterId,
                cancellationToken);

            if (disaster is null)
                return null;

            
            var teamIds = disaster.Teams.Select(t => t.TeamId).ToList();

            var teams = await _teamRepository.GetTeamsByIdsAsync(teamIds, cancellationToken);

            var teamDict = teams.ToDictionary(x => x.Id, x => x.Name);

            return new DisasterDetailsDto(
                disaster.Id,
                disaster.Title,
                disaster.Description,
                disaster.City,
                disaster.Province,
                disaster.Type.Name,
                disaster.Status.Name,
                disaster.Location.Latitude,
                  disaster.Location.Longitude,
                disaster.StartTime,
                disaster.EndTime,
                new ReporterDto(
                    disaster.Reporter.Name,
                    disaster.Reporter.Contact
                ),
                disaster.Teams.Select(t =>
                    new TeamDto(
                        t.TeamId,
                        teamDict.TryGetValue(t.TeamId, out var name) ? name : null
                    )
                ).ToList(),
                disaster.Resources.Select(r =>
                    new ResourceDto(
                        r.Id,
                        r.ResourceType.Name,
                        r.QuantitySent,
                        r.QuantityConsumed,
                        r.QuantityReturned,
                        r.QuantityDamaged,
                        null
                    )
                ).ToList(),
                disaster.AffectedPeople.Select(p =>
                    new AffectedPersonDto(
                       
                        p.Name,
                        p.Age,
                        p.Phone,
                        p.Status.Name,
                        p.Notes
                    )
                ).ToList(),
                disaster.Report is null
                    ? null
                    : new ReportDto(
                        disaster.Report.Id,
                        disaster.Report.Summary,
                        disaster.Report.PdfUrl,
                        disaster.Report.IssuedAt
                    )
            );
        }
    }
}
