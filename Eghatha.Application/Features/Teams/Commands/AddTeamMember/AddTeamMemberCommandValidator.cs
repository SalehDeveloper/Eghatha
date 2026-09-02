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
           .NotEmpty();



            RuleFor(x => x.FirstName)
                .NotEmpty()
                .MaximumLength(50);


            RuleFor(x => x.LastName)
                .NotEmpty()
                .MaximumLength(50);
               

            RuleFor(x => x.Email)
                .NotEmpty()
               
                .EmailAddress()
                
                .MaximumLength(150);

            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .Matches(@"^09\d{8}$");
                

            RuleFor(x => x.JobTitle)
                .NotEmpty()
                
                .MaximumLength(100);

          

            RuleFor(x => x.photo)
           .Must(file => file.Length > 0)
           .Must(file => file.Length <= 5 * 1024 * 1024);
      


        }
    }
}
