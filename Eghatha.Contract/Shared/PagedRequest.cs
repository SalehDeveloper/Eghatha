using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Contract.Shared
{
    public record class PagedRequest(int Page =1 , int PageSize=10);
    
    
}
