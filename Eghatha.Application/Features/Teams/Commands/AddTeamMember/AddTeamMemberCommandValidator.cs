using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Teams.Commands.AddTeamMember
{
    public class AddTeamMemberCommandValidator : AbstractValidator<AddTeamMemberCommand>
    {
        public AddTeamMemberCommandValidator()
        {
            RuleFor(x => x.TeamId)
           .NotEmpty()
           .WithMessage("TeamId is required.");

            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithMessage("First name is required.")
                .MaximumLength(50)
                .WithMessage("First name must not exceed 50 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage("Last name is required.")
                .MaximumLength(50)
                .WithMessage("Last name must not exceed 50 characters.");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required.")
                .EmailAddress()
                .WithMessage("Invalid email format.")
                .MaximumLength(150);

            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .WithMessage("Phone number is required.")
                .Matches(@"^\+?[0-9]{7,15}$")
                .WithMessage("Invalid phone number format.");

            RuleFor(x => x.JobTitle)
                .NotEmpty()
                .WithMessage("Job title is required.")
                .MaximumLength(100);

            RuleFor(x => x.PhotoUrl).NotEmpty();
            
               
        }
    }
}
