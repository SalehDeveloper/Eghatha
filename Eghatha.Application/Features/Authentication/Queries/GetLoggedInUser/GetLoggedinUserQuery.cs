using Eghatha.Application.Common.Models;
using Eghatha.Application.Features.Authentication.Dtos;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Authentication.Queries.GetLoggedInUser
{
    public sealed record GetLoggedinUserQuery:IRequest<ErrorOr<IdentityUser>>;
    
    
}
