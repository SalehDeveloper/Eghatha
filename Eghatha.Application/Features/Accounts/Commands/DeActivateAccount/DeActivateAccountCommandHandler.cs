using Eghatha.Application.Common.Authentication;
using Eghatha.Domain.Abstractions;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Accounts.Commands.DeActivateAccount
{
    public class DeActivateAccountCommandHandler : IRequestHandler<DeActivateAccountCommand, ErrorOr<Success>>
    {
        private readonly IIdentityService _identityService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly HybridCache _hybridCache;


        public DeActivateAccountCommandHandler(IIdentityService identityService, IUnitOfWork unitOfWork, HybridCache hybridCache)
        {
            _identityService = identityService;
            _unitOfWork = unitOfWork;
            _hybridCache = hybridCache;
        }

        public async Task<ErrorOr<Success>> Handle(DeActivateAccountCommand request, CancellationToken cancellationToken)
        {
            var res = await _identityService.DeActivateUser(request.Id);

            if (res.IsError)
                return res.Errors;

            await _unitOfWork.CompleteAsync(cancellationToken);

            await _hybridCache.RemoveByTagAsync("accounts", cancellationToken);

            return Result.Success;

        }
    }
}
