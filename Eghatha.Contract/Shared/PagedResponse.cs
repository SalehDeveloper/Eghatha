using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Contract.Shared
{
    public record PagedResponse<T>(int PageNumber, int PageSize, int TotalPages, int TotalCount, IReadOnlyCollection<T>? Items);
    
}
