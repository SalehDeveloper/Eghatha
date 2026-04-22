using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Infastructure.Data
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; } 
        public string PhotoUrl { get; private set; } 
        public bool IsActive {  get; private set; }

        public ApplicationUser(
       string firstName,
       string lastName,
       string email,
       string phoneNumber , 
       string photoUrl)
        {
            Id = Guid.NewGuid();

            FirstName = firstName;
            LastName = lastName;
            Email = email;
            UserName = email; 
            PhoneNumber = phoneNumber;
            PhotoUrl= photoUrl;
            IsActive = false;
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        public void Activate()
        {
            IsActive = true;
        }

    }
}
