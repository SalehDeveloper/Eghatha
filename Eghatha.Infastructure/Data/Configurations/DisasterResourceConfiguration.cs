using Eghatha.Domain.Disasters;
using Eghatha.Domain.Disasters.DisasterResources;
using Eghatha.Domain.Teams.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Infastructure.Data.Configurations
{
    public class DisasterResourceConfiguration : IEntityTypeConfiguration<DisasterResource>
    {
        public void Configure(EntityTypeBuilder<DisasterResource> builder)
        {
            builder.ToTable("DisasterResources");

            builder.HasKey(dr => dr.Id);

            builder.Property(dr => dr.DisasterId)
                .IsRequired();

            builder.Property(dr => dr.ResourceId)
                .IsRequired();

            builder.Property(dr => dr.TeamId)
                .IsRequired();

            builder.Property(dr => dr.QuantitySent)
                .IsRequired();

            builder.Property(dr => dr.QuantityConsumed)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(dr => dr.QuantityReturned)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(dr => dr.QuantityDamaged)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(dr => dr.AssignedAt)
                .IsRequired();

            builder.Property(dr => dr.Notes)
                .HasMaxLength(500);

            // Computed property - not mapped
            builder.Ignore(dr => dr.RemainingQuantity);

            // Relationships
            builder.HasOne<Disaster>()
                .WithMany(d => d.Resources)
                .HasForeignKey(dr => dr.DisasterId)
                .OnDelete(DeleteBehavior.Cascade);
          

            builder.HasOne<Resource>()
                .WithMany()
                .HasForeignKey(dr => dr.ResourceId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(dr => new { dr.DisasterId, dr.ResourceId });
          
        }
    }
}
