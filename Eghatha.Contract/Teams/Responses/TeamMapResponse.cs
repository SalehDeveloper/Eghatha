namespace Eghatha.Contract.Teams.Responses
{
    public record TeamMapResponse(
    Guid Id,
    string Name,
    string Speciality,
    string Status,
    double Latitude,
    double Longitude,
    bool IsLiveLocation,
    Guid? AssignedDisasterId
);


}
