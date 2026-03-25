using Eghatha.Domain.Teams;
using Eghatha.Domain.Teams.Resources;
using Eghatha.Domain.Volunteers.Equipments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Infastructure.Data.Configurations
{
    public class ResourceConfiguration : IEntityTypeConfiguration<Resource>
    {
        public void Configure(EntityTypeBuilder<Resource> builder)
        {
            builder.ToTable("Resources");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.TeamId)
                .IsRequired();

            builder.Property(r => r.Status)
                .HasConversion(
                v => v.Value,
                v => ResourceStatus.FromValue(v)).IsRequired();

            builder.Property(r => r.Quantity)
                .IsRequired();

            builder.Property(r => r.Type)
               .HasConversion(
               v => v.Value,
               v => ResourceType.FromValue(v)).IsRequired();


            // Relationships - One-to-Many with Team
            builder.HasOne<Team>()
                .WithMany(t => t.Resources)
                .HasForeignKey(r => r.TeamId)
                .OnDelete(DeleteBehavior.Cascade);

       
            builder.HasIndex(r => r.TeamId);
            builder.HasIndex(r => r.Type);
            builder.HasIndex(r => r.Status);
        }
    }
}
