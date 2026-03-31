using Eghatha.Domain.Identity;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Accounts.Queries.GetAccounts
{
    public class GetAccountsQueryValidator : AbstractValidator<GetAccountsQuery>
    {
        public GetAccountsQueryValidator()
        {
            RuleFor(x => x.Page)
           .GreaterThanOrEqualTo(1)
           .WithMessage("Page number must be at least 1")
           .LessThanOrEqualTo(1000)
           .WithMessage("Page number cannot exceed 1000");

            RuleFor(x => x.PageSize)
                .GreaterThanOrEqualTo(1)
                .WithMessage("Page size must be at least 1")
                .LessThanOrEqualTo(100)
                .WithMessage("Page size cannot exceed 100");

            RuleFor(x => x.SearchTearm)
                .MaximumLength(100)
                .WithMessage("Search term cannot exceed 100 characters")
                .When(x => !string.IsNullOrWhiteSpace(x.SearchTearm));

            // Role validation with enum
            RuleFor(x => x.Role)
                .Must(BeValidRole)
                .WithMessage($"Role must be one of: Admin, TeamMember, Volunteer ")
                .MaximumLength(50)
                .WithMessage("Role name cannot exceed 50 characters")
                .When(x => !string.IsNullOrWhiteSpace(x.Role));
        }

        private bool BeValidRole(string? role)
        {
            if (string.IsNullOrWhiteSpace(role))
                return true;

            return Enum.TryParse(typeof(Role), role, true, out _);
        }
    }

    }

