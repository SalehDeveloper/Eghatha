using Eghatha.Domain.Disasters;
using Eghatha.Domain.Disasters.AffectedPersons;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Infastructure.Data.Configurations
{
    public class AffectedPersonConfiguration : IEntityTypeConfiguration<AffectedPerson>
    {
        public void Configure(EntityTypeBuilder<AffectedPerson> builder)
        {
            builder.ToTable("AffectedPersons");

            builder.HasKey(ap => ap.Id);

            builder.Property(ap => ap.DisasterId)
                .IsRequired();

            builder.Property(ap => ap.Name)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(ap => ap.Age)
                .IsRequired();

            builder.Property(ap => ap.Phone)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(ap => ap.Status)
                .HasConversion(
                v => v.Value,
                v => HealthStatus.FromValue(v)).IsRequired();


            builder.Property(ap => ap.Notes)
                .HasMaxLength(500);

            // Relationships
            builder.HasOne<Disaster>()
                .WithMany(d => d.AffectedPeople)
                .HasForeignKey(ap => ap.DisasterId)
                .OnDelete(DeleteBehavior.Cascade);

            
            builder.HasIndex(ap => ap.Status);
        }
    }
}
