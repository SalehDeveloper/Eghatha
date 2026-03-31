using Eghatha.Application.Common.Models;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Accounts.Queries.GetAccounts
{
    public record GetAccountsQuery(
        int Page , 
        int PageSize , 
        string? SearchTearm,
        string? Role , 
        bool ? IsActive
        ) : ICachedQuery<PaginatedList<IdentityUser>>
    {
        public string CachKey =>
            $"accounts:p={Page}:ps={PageSize}" +
            $":q={SearchTearm ?? "-"}" +
            $":role={Role ?? "-"}" +
            $":active={IsActive.ToString() ?? "-"}";


        public string[] Tags => ["accounts"];

        public TimeSpan Expiration => TimeSpan.FromMinutes(10);
    }
}
