using Eghatha.Application.Common.Authentication;
using Eghatha.Application.Common.Interfaces;
using Eghatha.Domain.Abstractions;
using Eghatha.Domain.Identity;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Authentication.Commands.Login
{
    public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, ErrorOr<AppUserDto>>
    {
        private readonly IIdentityService _identityService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtService _jwtService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly TimeProvider _timeProvider;
        private readonly ICookieService _cookieService;

        public LoginCommandHandler(
            IIdentityService identityService,
            IUnitOfWork unitOfWork,
            IJwtService jwtService,
            IRefreshTokenRepository refreshTokenRepository,
            TimeProvider timeProvider,
            ICookieService cookieService)
        {
            _identityService = identityService;
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
            _refreshTokenRepository = refreshTokenRepository;
            _timeProvider = timeProvider;
            _cookieService = cookieService;
        }

        public async Task<ErrorOr<AppUserDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var authResult = await _identityService.AuthenticateAsync(request.Email, request.Password , cancellationToken);

            if (authResult.IsError) return authResult.Errors;

            var user = authResult.Value;

            var accessToken = _jwtService.GenerateAccessToken(user);
            var refresToken = _jwtService.GenerateRefreshToken();


            var refreshTokenEntity = Domain.Identity.RefreshToken.Create(user.UserId , refresToken , _timeProvider.GetUtcNow().AddDays(Domain.Identity.RefreshToken.RefreshTokenDurationInDays));

            if (refreshTokenEntity.IsError) return refreshTokenEntity.Errors;

            await _refreshTokenRepository.AddAsync(refreshTokenEntity.Value , cancellationToken);
        
            await _unitOfWork.CompleteAsync(cancellationToken);

            _cookieService.SetAccessTokenInCookies(accessToken);
            _cookieService.SetRefreshTokenInCookies(refresToken);

            return user;
        }
    }
}
