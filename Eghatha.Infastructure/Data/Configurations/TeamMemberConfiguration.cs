using Eghatha.Domain.Teams;
using Eghatha.Domain.Teams.TeamMembers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Infastructure.Data.Configurations
{
    public class TeamMemberConfiguration : IEntityTypeConfiguration<TeamMember>
    {
        public void Configure(EntityTypeBuilder<TeamMember> builder)
        {
            builder.ToTable("TeamMembers");

            builder.HasKey(tm => tm.Id);

            builder.Property(tm => tm.UserId)
                .IsRequired();

        

            builder.Property(tm => tm.JobTitle)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(tm => tm.IsLeader)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(tm => tm.Status)
             .HasConversion(
             v => v.Value,
             v => TeamMemberStatus.FromValue(v)).IsRequired();


            builder.Property(tm => tm.JoinedAt)
                .IsRequired();

            // Relationships
            builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(tm => tm.UserId)
                .OnDelete(DeleteBehavior.Restrict);


            // Indexes

            builder.HasIndex("TeamId", nameof(TeamMember.IsLeader))
           .HasDatabaseName("IX_TeamMembers_TeamId_IsLeader");

            builder.HasIndex(tm => tm.Status);
        }
    }

}
