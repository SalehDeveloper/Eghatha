using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Contract.Teams.Requests
{
    public record class AddTeamMemberRequest(string FirstName, string LastName, string Email, string PhoneNumber, IFormFile Photo , string JobTitle, bool IsLeader);

    
}
