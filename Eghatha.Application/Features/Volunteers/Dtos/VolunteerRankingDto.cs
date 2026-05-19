namespace Eghatha.Application.Features.Volunteers.Dtos
{
    public sealed record VolunteerRankingDto(
    Guid VolunteerId,
    string FullName,
    string Speciality,
    string Province,
    string City,
    int TotalMissions,
    int TotalScore,
    double AverageScore,
    int Rank);

}
