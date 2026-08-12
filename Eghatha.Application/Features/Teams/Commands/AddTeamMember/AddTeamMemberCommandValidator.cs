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
                .Matches(@"^09\d{8}$")
                .WithMessage("Phone number must be a valid Syrian mobile number (e.g. 0991234567).");

            RuleFor(x => x.JobTitle)
                .NotEmpty()
                .WithMessage("Job title is required.")
                .MaximumLength(100);

            //RuleFor(x => x.photo)
            //  .NotNull().WithMessage("Photo is required")
            //  .Must(file => file.Length > 0)
            //  .WithMessage("Photo cannot be empty")
            //  .Must(file => file.ContentType.StartsWith("image/"))
            //  .WithMessage("Photo must be an image")
            //  .Must(file => file.Length <= 5 * 1024 * 1024) 
            //  .WithMessage("Photo size must not exceed 5MB");

          RuleFor(x => x.photo)
         .NotNull().WithMessage("Photo is required")
         .Must(file => file.Length > 0)
         .WithMessage("Photo cannot be empty")
         .Must(file => file.Length <= 5 * 1024 * 1024)
         .WithMessage("Photo size must not exceed 5MB");


        }
    }
}
