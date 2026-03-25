using Eghatha.Domain.Teams;
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
    public class TeamConfiguration : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            builder.ToTable("Teams");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Name)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(t => t.Speciality)
               .HasConversion(
               v => v.Value,
               v => TeamSpeciality.FromValue(v)).IsRequired();

            builder.Property(t => t.Province)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(t => t.City)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(t => t.Status)
             .HasConversion(
             v => v.Value,
             v => TeamStatus.FromValue(v)).IsRequired();


            builder.Property(t => t.CreatedByAdminId)
                .IsRequired();

            // Configure Value Object - GeoLocation
            builder.OwnsOne(t => t.Location, location =>
            {
                location.Property(l => l.Latitude)
                    .HasColumnName("BaseLocationLat")
                    .HasPrecision(10, 8)
                    .IsRequired();

                location.Property(l => l.Longitude)
                    .HasColumnName("BaseLocationLng")
                    .HasPrecision(11, 8)
                    .IsRequired();
            });

            // Relationships
            builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(t => t.CreatedByAdminId)
                .OnDelete(DeleteBehavior.Restrict);


            builder.Navigation(t => t.Members)
             .HasField("_members")
              .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.Navigation(t => t.Resources)
                .HasField("_resources")
                .UsePropertyAccessMode(PropertyAccessMode.Field);
            // Ignore computed properties
            builder.Ignore(t => t.IsReadyForMission);
            builder.Ignore(t => t.Leader);

            // Indexes
        
            builder.HasIndex(t => t.Status);
            builder.HasIndex(t => t.Speciality);
        
        }
    }
}
