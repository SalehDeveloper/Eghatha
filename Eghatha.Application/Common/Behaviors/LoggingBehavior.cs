using Eghatha.Application.Common.Authentication;
using Eghatha.Application.Common.Interfaces;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Common.Behaviors
{
    public class LoggingBehavior<TRequest>(ILogger<TRequest> logger, IUser user, IIdentityService identityService)
    : IRequestPreProcessor<TRequest>
    where TRequest : notnull
    {
        private readonly ILogger _logger = logger;
        private readonly IUser _user = user;
        private readonly IIdentityService _identityService = identityService;

        public async Task Process(TRequest request, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            var userId = _user.Id ?? Guid.Empty;
            string? userName = string.Empty;

            if (userId != Guid.Empty)
            {
                userName = await _identityService.GetUserNameAsync(userId);
            }

            _logger.LogInformation(
                "Request: {Name} {@UserId} {@UserName} {@Request}", requestName, userId, userName, request);
        }

    }
}
