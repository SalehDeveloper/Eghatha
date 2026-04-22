using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Features.Teams.Commands.AddTeamMember
{
    public record AddTeamMemberCommand(Guid TeamId , string FirstName , string LastName,string Email , string PhoneNumber, string PhotoUrl , string JobTitle , bool IsLeader) : IRequest<ErrorOr<Updated>>;
    
    
}
