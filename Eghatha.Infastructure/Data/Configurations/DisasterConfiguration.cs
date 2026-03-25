using Eghatha.Domain.Disasters;
using Eghatha.Domain.Disasters.AffectedPersons;
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
    public class DisasterConfiguration : IEntityTypeConfiguration<Disaster>
    {
        public void Configure(EntityTypeBuilder<Disaster> builder)
        {
            builder.ToTable("Disasters");

            builder.HasKey(d => d.Id);

            builder.Property(d => d.Title)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(d => d.Description)
                .HasMaxLength(2000)
                .IsRequired();

            builder.Property(d => d.StartTime)
                .IsRequired();

            builder.Property(d => d.EndTime);

            builder.Property(d => d.Status)
                .HasConversion(
                v => v.Value,
                v => DisasterStatus.FromValue(v)).IsRequired();





            builder.Property(d => d.Type)
                .HasConversion(
                v => v.Value,
                v => DisasterType.FromValue(v)).IsRequired();

            builder.Property(d => d.CustomTypeDescription)
                .HasMaxLength(200);

            // Configure Value Object - GeoLocation
            builder.OwnsOne(d => d.Location, location =>
            {
                location.Property(l => l.Latitude)
                    .HasColumnName("LocationLat")
                    .HasPrecision(10, 8)
                    .IsRequired();

                location.Property(l => l.Longitude)
                    .HasColumnName("LocationLng")
                    .HasPrecision(11, 8)
                    .IsRequired();
            });

            // Configure Value Object - ReporterInfo
            builder.OwnsOne(d => d.Reporter, reporter =>
            {
                reporter.Property(r => r.Name)
                    .HasColumnName("ReporterName")
                    .HasMaxLength(100)
                    .IsRequired();

                reporter.Property(r => r.IdNumber)
                    .HasColumnName("ReporterIdNumber")
                    .HasMaxLength(50);

                reporter.Property(r => r.Contact)
                    .HasColumnName("ReporterContact")
                    .HasMaxLength(50)
                    .IsRequired();
            });

            // Configure Report (owned entity - one-to-one)
            builder.OwnsOne(d => d.Report, report =>
            {
                report.Property(r => r.Summary)
                    .HasColumnName("ReportSummary")
                    .HasMaxLength(2000)
                    .IsRequired();

                report.Property(r => r.Teams)
                    .HasColumnName("ReportTeams")
                    .HasMaxLength(2000)
                    .IsRequired();

                report.Property(r => r.Resources)
                    .HasColumnName("ReportResources")
                    .HasMaxLength(2000)
                    .IsRequired();

                report.Property(r => r.AffectedPersons)
                    .HasColumnName("ReportAffectedPersons")
                    .HasMaxLength(2000)
                    .IsRequired();

                report.Property(r => r.IssuedAt)
                    .HasColumnName("ReportIssuedAt")
                    .IsRequired();
            });



           

            builder.Navigation(d => d.Volunteers)
            .HasField("_volunteers")
             .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.Navigation(d => d.Resources)
                .HasField("_resources")
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.Navigation(d => d.AffectedPeople)
                .HasField("_affectedPeople")
                .UsePropertyAccessMode(PropertyAccessMode.Field);


            // Indexes
            builder.HasIndex(d => d.Status);
            builder.HasIndex(d => d.Type);
           
        }
    }
}
