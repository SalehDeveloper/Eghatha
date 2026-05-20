using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Volunteers.Queries.GetEquipmentCategories
{
    public sealed record GetEquipmentCategoriesQuery
       : IRequest<IReadOnlyList<EquipmentCategoryResponse>>;

    public sealed record EquipmentCategoryResponse(int Value, string Name);
}
