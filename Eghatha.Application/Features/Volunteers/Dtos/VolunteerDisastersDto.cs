namespace Eghatha.Application.Features.Volunteers.Dtos
{
    public sealed record VolunteerDisastersDto(Guid DisasterId, string Title,
     string City,
     string Province,
     double Latitude,
     double Longitude,
     string Type,
     string Status,
    DateTimeOffset StartTime);

}
