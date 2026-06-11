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
    public class GetLoggedinUserQueryHandler: IRequestHandler<GetLoggedinUserQuery, ErrorOr<AppUserDto>>
    {
        
        private readonly IIdentityService _identityService;

        public GetLoggedinUserQueryHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<ErrorOr<AppUserDto>> Handle(GetLoggedinUserQuery request, CancellationToken cancellationToken)
        {
          

            var user = await _identityService.GetUserByIdAsync(  request.UserId, cancellationToken);

            if (user.IsError) return user.Errors;

            return user.Value;

           
            
        }
    }
}
