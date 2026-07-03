namespace Eghatha.Application.Features.Disasters.Dtos
{
    public sealed record DisasterVolunteerReportDto(
     Guid VolunteerId,
     string FullName,
     string Email,
     string? Phone,
     int? TotalScore,
     double? AverageScore,
     string? Notes);
}
