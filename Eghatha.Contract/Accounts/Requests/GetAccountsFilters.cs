using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Contract.Accounts.Requests
{
    public record GetAccountsFilters(
       string? SearchTerm,
       string? Role,
        bool? IsActive
    );
    
    
}
