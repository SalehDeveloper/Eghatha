using Eghatha.Application.Common.Authentication;
using Eghatha.Application.Common.Interfaces;
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
    public class GetLoggedinUserQueryHandler : IRequestHandler<GetLoggedinUserQuery, ErrorOr<MeDto>>
    {
        private readonly IUser _user;
        private readonly IIdentityService _identityService;

        public GetLoggedinUserQueryHandler(IUser user, IIdentityService identityService)
        {
            _user = user;
            _identityService = identityService;
        }

        public async Task<ErrorOr<MeDto>> Handle(GetLoggedinUserQuery request, CancellationToken cancellationToken)
        {
            var userId = _user.Id;

            var user = await _identityService.GetUserDetailsByIdAsync(userId , cancellationToken);

            if (user.IsError) return user.Errors;



            return new MeDto(user.Value.Id, user.Value.Email, user.Value.Roles, user.Value.PhoneNumber, user.Value.FirstName, user.Value.LastName, user.Value.PhotoUrl);
            
        }
    }
}
