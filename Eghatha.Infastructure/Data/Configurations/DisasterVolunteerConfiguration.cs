using Eghatha.Domain.Disasters;
using Eghatha.Domain.Disasters.DisasterVolunteers;
using Eghatha.Domain.Teams.TeamMembers;
using Eghatha.Domain.Volunteers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Infastructure.Data.Configurations
{
    public class DisasterVolunteerConfiguration : IEntityTypeConfiguration<DisasterVolunteer>
    {
        public void Configure(EntityTypeBuilder<DisasterVolunteer> builder)
        {
            builder.ToTable("DisasterVolunteers");

            builder.HasKey(dv => dv.Id);

            builder.Property(dv => dv.VolunteerId)
                .IsRequired();

            builder.Property(dv => dv.DisasterId)
                .IsRequired();
            

            builder.OwnsOne(dv => dv.EvaluationScores, scores =>
            {
                scores.Property(s => s.CommitmentScore)
                    .HasColumnName("CommitmentScore")
                    .IsRequired();

                scores.Property(s => s.skillScore)
                    .HasColumnName("SkillScore")
                    .IsRequired();

                scores.Property(s => s.SafetyScore)
                    .HasColumnName("SafetyScore")
                    .IsRequired();

                scores.Property(s => s.TeamWorkScore)
                    .HasColumnName("TeamworkScore")
                    .IsRequired();

                scores.Property(s => s.InitiativeScore)
                    .HasColumnName("InitiativeScore")
                    .IsRequired();

           
            });
  
        
            builder.Property(dv => dv.Notes)
                .HasMaxLength(500);

            builder.Property(dv => dv.EvaluatedAt);

            builder.Property(dv => dv.EvaluatedByLeaderId);

            // Relationships
            builder.HasOne<Volunteer>()
                .WithMany()
                .HasForeignKey(dv => dv.VolunteerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Disaster>()
                .WithMany(d => d.Volunteers)
                .HasForeignKey(dv => dv.DisasterId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<TeamMember>()
          .WithMany()
          .HasForeignKey(dv => dv.EvaluatedByLeaderId)
          .OnDelete(DeleteBehavior.Restrict)
          .IsRequired(false);

            // Indexes
            builder.HasIndex(dv => new { dv.DisasterId, dv.VolunteerId }).IsUnique();
           
        }
    }
}
