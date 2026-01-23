
namespace Eghatha.Domain.Abstractions
{
    public record QueryOptions(
        int PageNumber = 1,
        int PageSize = 10,
        string? OrderBy = null,
        SortOrder SortOrder = SortOrder.Ascending
    );
}
