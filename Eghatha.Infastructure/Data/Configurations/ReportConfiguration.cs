using Eghatha.Domain.Disasters.Reports;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Infastructure.Data.Configurations
{
    public class ReportConfiguration : IEntityTypeConfiguration<Report>
    {
        public void Configure(EntityTypeBuilder<Report> builder)
        {
           

            builder.ToTable("Reports");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Summary)
                   .HasMaxLength(2000)
                   .IsRequired();

            builder.Property(r => r.PdfUrl)
                .HasMaxLength(1000)
                .IsRequired();

            builder.Property(r => r.IssuedAt)
                .IsRequired();

            builder.HasIndex(r => r.DisasterId)
                .IsUnique();
        }
    }
}
