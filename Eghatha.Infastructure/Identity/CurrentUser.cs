using Eghatha.Application.Common.Interfaces;
using Eghatha.Domain.Identity;
using Eghatha.Infastructure.Identity;
using Microsoft.AspNetCore.Http;

namespace Eghatha.Api.Services
{
    public class CurrentUser : IUser
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public CurrentUser(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public Guid Id => _contextAccessor.HttpContext?
            .User.GetUserId()
            ?? throw new ApplicationException("User context is unavailable");

        public Role Role => _contextAccessor.HttpContext?.User.GetUserRole() 
           ?? throw new ApplicationException("User context is unavailable");
    }
}
