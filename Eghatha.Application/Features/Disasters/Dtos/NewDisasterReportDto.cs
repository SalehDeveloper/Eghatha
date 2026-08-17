namespace Eghatha.Application.Features.Disasters.Dtos
{
    public record NewDisasterReportDto(
        Guid DisasterId,
        string Title,
        string Description,
        double Latitude,
        double Longitude,
        DateTimeOffset StartTime);
}
