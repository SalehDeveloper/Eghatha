using Eghatha.Application.Common.Authentication;
using Eghatha.Application.Common.Errors;
using Eghatha.Application.Common.Interfaces;
using Eghatha.Domain.Abstractions;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Authentication.Commands.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, ErrorOr<Success>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly TimeProvider _timeProvider;
        private readonly ICookieService _cookieService;
        private readonly IJwtService _jwtService;
        private readonly IIdentityService _identityService;

        public RefreshTokenCommandHandler(
            IUnitOfWork unitOfWork,
            IRefreshTokenRepository refreshTokenRepository,
            TimeProvider timeProvider,
            ICookieService cookieService,
            IJwtService jwtService,
            IIdentityService identityService)
        {
            _unitOfWork = unitOfWork;
            _refreshTokenRepository = refreshTokenRepository;
            _timeProvider = timeProvider;
            _cookieService = cookieService;
            _jwtService = jwtService;
            _identityService = identityService;
        }

        public async Task<ErrorOr<Success>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
           
            var refreshToken = _cookieService.GetRefreshToken();

            if(string.IsNullOrEmpty(refreshToken))
                  return ApplicationErrors.InvalidRefreshToken;

            var token = await _refreshTokenRepository.GetByTokenAsync(refreshToken , cancellationToken);

            if (token == null)
                return ApplicationErrors.InvalidRefreshToken;

            if (token.ExpiresOnUtc <= _timeProvider.GetUtcNow())
                return ApplicationErrors.InvalidRefreshToken;

           
            if (token.IsRevoked)
            {
                await _refreshTokenRepository.RevokeAllByUserId(token.UserId, cancellationToken);
                await _unitOfWork.CompleteAsync(cancellationToken);

                return ApplicationErrors.InvalidRefreshToken;
            }

            var user = await _identityService.GetUserByIdAsync(token.UserId, cancellationToken);

            if (user.IsError )
                return ApplicationErrors.InvalidRefreshToken;


            token.Revoke();

            var newRefreshToken = _jwtService.GenerateRefreshToken();

            var newTokenEntity = Domain.Identity.RefreshToken.Create( token.UserId, newRefreshToken ,_timeProvider.GetUtcNow().AddDays(Domain.Identity.RefreshToken.RefreshTokenDurationInDays));
                

            if (newTokenEntity.IsError)
                return newTokenEntity.Errors;

            await _refreshTokenRepository.AddAsync(newTokenEntity.Value, cancellationToken);

            
            var accessToken = _jwtService.GenerateAccessToken(user.Value);

           
            await _unitOfWork.CompleteAsync(cancellationToken);

            // 8. Set cookies AFTER success
            _cookieService.SetRefreshTokenInCookies(newRefreshToken);
            _cookieService.SetAccessTokenInCookies(accessToken);

            return Result.Success;
        }
    }
}
