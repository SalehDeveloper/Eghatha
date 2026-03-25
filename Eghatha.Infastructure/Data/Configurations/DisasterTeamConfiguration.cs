using Eghatha.Domain.Disasters;
using Eghatha.Domain.Teams;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Infastructure.Data.Configurations
{
    public class DisasterTeamConfiguration : IEntityTypeConfiguration<DisasterTeam>
    {
        public void Configure(EntityTypeBuilder<DisasterTeam> builder)
        {
            builder.ToTable("DisasterTeams");

            // Composite primary key
            builder.HasKey(dt => new { dt.DisasterId, dt.TeamId });

           

            builder.HasOne<Disaster>()
                .WithMany()
                .HasForeignKey(dt => dt.DisasterId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<Team>()
                .WithMany()
                .HasForeignKey(dt => dt.TeamId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

