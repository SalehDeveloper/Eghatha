using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.VolunteerRegisterations.Commands.RejectRegisteration
{
    public class RejectRegisterationCommandValidator:AbstractValidator<RejectRegisterationCommand>
    {
        public RejectRegisterationCommandValidator()
        {

            RuleFor(x => x.RegisterationId).NotEmpty().NotNull();

            RuleFor(x => x.Reason).NotEmpty().NotNull();

        }
    }
}
