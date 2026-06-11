using Eghatha.Application.Common.Errors;
using Eghatha.Application.Common.Interfaces;
using Eghatha.Application.Common.Services;
using Eghatha.Application.Features.Disasters.Dtos;
using Eghatha.Domain.Abstractions;
using Eghatha.Domain.Disasters;
using Eghatha.Domain.Disasters.Reports;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Disasters.Commands.GenerateReport
{
    public class GenerateDisasterReportCommandHandler : IRequestHandler<GenerateDisasterReportCommand, ErrorOr<GenerateDisasterReportDto>>
    {
        private readonly IDisasterRepository _disasterRepository;
        private readonly IDisasterReportPdfService _disasterReportPdfService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly TimeProvider _timeProvider;
        private readonly HybridCache _hybridCache;




        public GenerateDisasterReportCommandHandler(IDisasterRepository disasterRepository, IDisasterReportPdfService disasterReportPdfService, IUnitOfWork unitOfWork, ICloudinaryService cloudinaryService, TimeProvider timeProvider, HybridCache hybridCache)
        {
            _disasterRepository = disasterRepository;
            _disasterReportPdfService = disasterReportPdfService;
            _unitOfWork = unitOfWork;
            _cloudinaryService = cloudinaryService;
            _timeProvider = timeProvider;
            _hybridCache = hybridCache;
        }

        public async Task<ErrorOr<GenerateDisasterReportDto>> Handle(GenerateDisasterReportCommand request, CancellationToken cancellationToken)
        {
            var disaster = await _disasterRepository.GetByIdWithAllDetailsAsync(request.DisasterId , cancellationToken);

            if (disaster is null)
                return ApplicationErrors.DisasterNotFound;

            if (disaster.Report is not null)
                return DisasterErrors.ReportAlreadyExists;

            if (disaster.Status != DisasterStatus.Closed)
                return DisasterErrors.CannotGenerateReportWhenDisasterNotClosed;


            var pdfBytes = await _disasterReportPdfService.Generate(disaster , cancellationToken);

            var uploadResult = await _cloudinaryService.UploadDisasterReportAsync(disaster.Id, pdfBytes);

            if (uploadResult.IsError)
                return uploadResult.Errors;

            var reportResult = Report.Create(
            Guid.NewGuid(),
            disaster.Id,
            BuildSummary(disaster),
            uploadResult.Value,
            _timeProvider.GetUtcNow());

            if (reportResult.IsError)
                return reportResult.Errors;

            var addReportResult = disaster.AddReport(reportResult.Value);

            if (addReportResult.IsError)
                return addReportResult.Errors;

            await _disasterRepository.AddReportAsync(addReportResult.Value, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            await _hybridCache.RemoveByTagAsync("disasters");
            return new GenerateDisasterReportDto(uploadResult.Value);

        }

        private string BuildSummary(Disaster disaster)
        {
            return $"Disaster '{disaster.Title}' handled in {disaster.City}. " +
                   $"Teams: {disaster.Teams.Count}, " +
                   $"Resources: {disaster.Resources.Count}, " +
                   $"Affected: {disaster.AffectedPeople.Count}";
        }
    }
}
