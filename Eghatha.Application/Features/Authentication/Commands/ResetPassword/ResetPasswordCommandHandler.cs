using Eghatha.Application.Common.Authentication;
using Eghatha.Application.Common.Errors;
using Eghatha.Application.Common.Models;
using Eghatha.Application.Common.Services;
using Eghatha.Domain.Abstractions;
using ErrorOr;
using MediatR;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Authentication.Commands.ResetPassword
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, ErrorOr<string>>
    {
        private readonly IIdentityService _identityService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOtpService _otpService;

        public ResetPasswordCommandHandler(IIdentityService identityService, IUnitOfWork unitOfWork, IOtpService otpService)
        {
            _identityService = identityService;
            _unitOfWork = unitOfWork;
            _otpService = otpService;
        }

        public async Task<ErrorOr<string>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {

            var user = await _identityService.GetIdentityUserByEmailAsync(request.Email, cancellationToken);

            if (user.IsError) return user.Errors;

            var res = await _otpService.ValidateAsync(OtpType.ResetPassword, request.Email, request.Otp);

            if (res.IsError) return res.Errors;

            var resetPasswordResult = await _identityService.ResetPasswordAsync(request.Email  , request.NewPassword , cancellationToken);

            await _otpService.RemoveAsync(OtpType.ResetPassword, request.Email);

            await _unitOfWork.CompleteAsync(cancellationToken);
           
            return "Password reseted successful";
        }
    }
}
