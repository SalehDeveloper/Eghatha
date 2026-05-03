namespace Eghatha.Contract.Volunteers.Requests
{
    public sealed record UpdateEquipmentRequest(
        string? Name,
        string? Category,
        string? Status,
        int? Quantity
    );
}
