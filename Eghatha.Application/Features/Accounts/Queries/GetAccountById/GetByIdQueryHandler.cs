using Eghatha.Application.Common.Authentication;
using Eghatha.Application.Common.Models;
using ErrorOr;
using MediatR;

namespace Eghatha.Application.Features.Accounts.Queries.GetAccountById
{
    public sealed class GetByIdQueryHandler : IRequestHandler<GetByIdQuery, ErrorOr<IdentityUser>>
    {
        private readonly IIdentityService identityService;

        public GetByIdQueryHandler(IIdentityService identityService)
        {
            this.identityService = identityService;
        }

        public async Task<ErrorOr<IdentityUser>> Handle(GetByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await identityService.GetUserDetailsByIdAsync(request.Id, cancellationToken);

            if (user.IsError) return user.Errors;

            return user;
        }
    }
}
