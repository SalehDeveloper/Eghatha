namespace Eghatha.Contract.Disasters.Requests
{
    public sealed record UpdateAffectedPersonRequest(
    string Name,
    int Age,
    string Phone,
    string Status,
    string? Notes);


}
