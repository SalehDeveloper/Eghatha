using Eghatha.Domain.Teams;
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
    public class VolunteerConfiguration : IEntityTypeConfiguration<Volunteer>
    {
        public void Configure(EntityTypeBuilder<Volunteer> builder)
        {
            builder.ToTable("Volunteers");

            builder.HasKey(v => v.Id);

            builder.Property(v => v.UserId)
                .IsRequired();

            // Configure Value Object - GeoLocation
            builder.OwnsOne(v => v.Location, location =>
            {
                location.Property(l => l.Latitude)
                    .HasColumnName("CurrentLat")
                    .HasPrecision(10, 8)
                    .IsRequired();

                location.Property(l => l.Longitude)
                    .HasColumnName("CurrentLng")
                    .HasPrecision(11, 8)
                    .IsRequired();
            });

            // Enum conversions
            builder.Property(v => v.Speciality)
              .HasConversion(
              v => v.Value,
              v => VolunteerSpeciality.FromValue(v)).IsRequired();

            builder.Property(v => v.Status)
              .HasConversion(
              v => v.Value,
              v => VolunteerStatus.FromValue(v)).IsRequired();

            builder.Property(v => v.YearsOfExperience)
                .IsRequired();

            builder.Property(v => v.Cv)
                .HasMaxLength(500);

            builder.Property(v => v.TotalMissions)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(v => v.TotalScore)
                .IsRequired()
                .HasDefaultValue(0);

            // Relationships
            builder.HasOne<ApplicationUser>()
                .WithOne()
                .HasForeignKey<Volunteer>(v => v.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Navigation(v => v.Equipments)
              .HasField("_equipments")
             .UsePropertyAccessMode(PropertyAccessMode.Field);
            // Computed property - not mapped
            builder.Ignore(v => v.AverageScore);

            // Indexes
            builder.HasIndex(v => v.UserId).IsUnique();
            
            builder.HasIndex(v => v.Speciality);
        }
    }
}
