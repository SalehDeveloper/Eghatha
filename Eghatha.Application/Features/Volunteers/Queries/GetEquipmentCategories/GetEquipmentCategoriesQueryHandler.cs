using Eghatha.Domain.Volunteers.Equipments;
using MediatR;

namespace Eghatha.Application.Features.Volunteers.Queries.GetEquipmentCategories
{
    public sealed class GetEquipmentCategoriesQueryHandler
    : IRequestHandler<GetEquipmentCategoriesQuery, IReadOnlyList<EquipmentCategoryResponse>>
    {
        public Task<IReadOnlyList<EquipmentCategoryResponse>> Handle(
            GetEquipmentCategoriesQuery request,
            CancellationToken cancellationToken)
        {
            var result = EquipmentCategory.List
                .Select(x => new EquipmentCategoryResponse(x.Value, x.Name))
                .ToList();

            return Task.FromResult<IReadOnlyList<EquipmentCategoryResponse>>(result);
        }
    }
}
