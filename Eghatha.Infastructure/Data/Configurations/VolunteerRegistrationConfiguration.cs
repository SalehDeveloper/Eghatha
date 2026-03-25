using Eghatha.Domain.VolunteerRegisterations;
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
    public class VolunteerRegistrationConfiguration : IEntityTypeConfiguration<VolunteerRegisteration>
    {
        public void Configure(EntityTypeBuilder<VolunteerRegisteration> builder)
        {
            builder.ToTable("VolunteerRegistrations");

            builder.HasKey(vr => vr.Id);

            builder.Property(vr => vr.VolunteerId)
                .IsRequired();

            builder.Property(vr => vr.Status)
              .HasConversion(
              v => v.Value,
              v => RegisterationStatus.FromValue(v)).IsRequired();


            builder.Property(vr => vr.RequestedAt)
                .IsRequired();

            builder.Property(vr => vr.ReviewedAt);

            builder.Property(vr => vr.ReviewedByAdminId);

            // Relationships - One-to-One with Volunteer
            builder.HasOne<Volunteer>()
                .WithOne()
                .HasForeignKey<VolunteerRegisteration>(vr => vr.VolunteerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(vr => vr.VolunteerId).IsUnique();
            builder.HasIndex(vr => vr.Status);
          
        }
    }
}
