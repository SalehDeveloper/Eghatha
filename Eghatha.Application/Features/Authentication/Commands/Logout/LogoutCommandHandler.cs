using Eghatha.Application.Common.Authentication;
using Eghatha.Application.Common.Interfaces;
using Eghatha.Domain.Abstractions;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Authentication.Commands.Logout
{
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, ErrorOr<Success>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly ICookieService _cookieService;

        public LogoutCommandHandler(IUnitOfWork unitOfWork, IRefreshTokenRepository refreshTokenRepository, ICookieService cookieService)
        {
            _unitOfWork = unitOfWork;
            _refreshTokenRepository = refreshTokenRepository;
            _cookieService = cookieService;
        }

        public async Task<ErrorOr<Success>> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            var refreshToken = _cookieService.GetRefreshToken();

            if (!string.IsNullOrEmpty(refreshToken))
            {
                var token = await _refreshTokenRepository.GetByTokenAsync(refreshToken, cancellationToken);

                if (token is not null && !token.IsRevoked)
                {
                    token.Revoke();
                    await _unitOfWork.CompleteAsync(cancellationToken);
                }
            }

            
            _cookieService.DeleteAccessToken();
            _cookieService.DeleteRefreshToken();

            return Result.Success;
        }
    }
}
