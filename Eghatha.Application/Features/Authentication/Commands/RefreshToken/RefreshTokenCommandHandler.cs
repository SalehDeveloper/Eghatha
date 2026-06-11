using Eghatha.Application.Common.Authentication;
using Eghatha.Application.Common.Errors;
using Eghatha.Application.Common.Interfaces;
using Eghatha.Application.Features.Authentication.Dtos;
using Eghatha.Domain.Abstractions;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Authentication.Commands.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, ErrorOr<TokenResponse>>
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

        public async Task<ErrorOr<TokenResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {

            var principal = _jwtService.GetPrincipalFromExpiredToken(request.ExpiredAccessToken);

            if (principal is null)
            {
         
                return ApplicationErrors.ExpiredAccessTokenInvalid;
            }


            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId is null)
            {
              
                return ApplicationErrors.UserIdClaimInvalid;
            }

            // get user by id 

            var user = await _identityService.GetUserByIdAsync(Guid.Parse(userId), cancellationToken);

            if (user.IsError) return user.Errors;



            var refreshToken = await _refreshTokenRepository.GetTokenForUserAsync(request.RefreshToken, user.Value.UserId);

            if (refreshToken is null || refreshToken.ExpiresOnUtc < _timeProvider.GetUtcNow() || refreshToken.IsRevoked)
            {
                return ApplicationErrors.RefreshTokenExpired;

            }

            // revoke all refresh tokens for the user to prevent reuse of any existing tokens

            await _refreshTokenRepository.RevokeAllByUserId(user.Value.UserId, cancellationToken);

            // genereate nwe access token , refresh token 

            var newAccessToken = _jwtService.GenerateAccessToken(user.Value);

            var newRefreshToken = _jwtService.GenerateRefreshToken();

            var newTokenEntity = Domain.Identity.RefreshToken.Create(user.Value.UserId, newRefreshToken, _timeProvider.GetUtcNow().AddDays(Domain.Identity.RefreshToken.RefreshTokenDurationInDays));


            if (newTokenEntity.IsError)
                return newTokenEntity.Errors;

            await _refreshTokenRepository.AddAsync(newTokenEntity.Value, cancellationToken);

            await _unitOfWork.CompleteAsync(cancellationToken);

            return new TokenResponse
            {
                AccessToken = newAccessToken.Token , 
                RefreshToken = newRefreshToken,
                ExpiresOnUtc = newAccessToken.Expires
            }

           ;
        }
    }
}
