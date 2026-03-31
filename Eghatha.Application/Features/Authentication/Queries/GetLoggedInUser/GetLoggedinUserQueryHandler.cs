using Eghatha.Application.Common.Authentication;
using Eghatha.Application.Common.Interfaces;
using Eghatha.Application.Common.Models;
using Eghatha.Application.Features.Authentication.Dtos;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Authentication.Queries.GetLoggedInUser
{
    public class GetLoggedinUserQueryHandler : IRequestHandler<GetLoggedinUserQuery, ErrorOr<IdentityUser>>
    {
        private readonly IUser _user;
        private readonly IIdentityService _identityService;

        public GetLoggedinUserQueryHandler(IUser user, IIdentityService identityService)
        {
            _user = user;
            _identityService = identityService;
        }

        public async Task<ErrorOr<IdentityUser>> Handle(GetLoggedinUserQuery request, CancellationToken cancellationToken)
        {
            var userId = _user.Id;

            var user = await _identityService.GetUserDetailsByIdAsync(userId.Value , cancellationToken);

            if (user.IsError) return user.Errors;

            return user.Value;

           
            
        }
    }
}
