using Eghatha.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Common.Interfaces
{
    public interface  IUser
    {
        public Guid? Id { get; }

        public List<string> Role { get; }
    }
}
