using Eghatha.Application.Common.Models;
using Eghatha.Application.Features.Authentication.Dtos;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Accounts.Queries.GetAccountById
{
    public sealed record GetByIdQuery(Guid Id) : IRequest<ErrorOr<IdentityUser>>;
}
