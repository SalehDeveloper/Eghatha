using Eghatha.Application.Common.Authentication;
using Eghatha.Application.Common.Errors;
using Eghatha.Application.Common.Models;
using Eghatha.Application.Common.Services;
using Eghatha.Domain.Abstractions;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Authentication.Commands.ConfirmEmail
{
    public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, ErrorOr<string>>
    {
        private readonly IIdentityService _identityService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOtpService _otpService;

        public ConfirmEmailCommandHandler(IIdentityService identityService, IUnitOfWork unitOfWork, IOtpService otpService)
        {
            _identityService = identityService;
            _unitOfWork = unitOfWork;
            _otpService = otpService;
        }

        public async Task<ErrorOr<string>> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
           

            var user = await _identityService.GetIdentityUserByEmailAsync(request.Email , cancellationToken);
           
            if (user.IsError) return user.Errors;

            if (user.Value.IsEmailConfirmed)
                return ApplicationErrors.EmailAlreadyConfirmed;

            var res = await _otpService.ValidateAsync(OtpType.ConfirmEmail, request.Email , request.Otp );

            if (res.IsError) return res.Errors;

            if (user.Value.Roles.Any(r => r == "Volunteer"))
            {
                await _identityService.ConfirmEmail(user.Value.Email);
            }
            else
            {
                await _identityService.ConfirmEmail(user.Value.Email);
                await _identityService.ActivateUser(user.Value.Id);
            }   
  
            await _otpService.RemoveAsync(OtpType.ConfirmEmail , request.Email);


            await _unitOfWork.CompleteAsync(cancellationToken);
          
            return $"Email confirmed Successfully , now you can login to your account";
        }
    }
}
