using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Domain.Disasters
{
    public sealed class DisasterTeam
    {
        public Guid DisasterId { get; private set; }
        public Guid TeamId { get; private set; }

        private DisasterTeam() { }

        public DisasterTeam(Guid disasterId, Guid teamId)
        {
            DisasterId = disasterId;
            TeamId = teamId;
        }
    }
}
