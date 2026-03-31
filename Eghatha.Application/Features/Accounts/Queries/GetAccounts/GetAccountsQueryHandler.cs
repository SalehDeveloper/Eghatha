using Eghatha.Application.Common.Authentication;
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
    public class GetAccountsQueryHandler : IRequestHandler<GetAccountsQuery, PaginatedList<IdentityUser>>
    {
        private readonly IIdentityService _identityService;

        public GetAccountsQueryHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<PaginatedList<IdentityUser>> Handle(GetAccountsQuery request, CancellationToken cancellationToken)
        {
          var res =   await _identityService.GetAccountsAsync(request.Page, request.PageSize, request.SearchTearm, request.Role, request.IsActive , cancellationToken);
           
          return res;
        }
    }
}
