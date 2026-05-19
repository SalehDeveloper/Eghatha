using Eghatha.Domain.Disasters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eghatha.Infastructure.Data.Configurations
{
    public sealed class DisasterTimelineEventConfiguration
    : IEntityTypeConfiguration<DisasterTimeLineEvent>
    {
        public void Configure(EntityTypeBuilder<DisasterTimeLineEvent> builder)
        {
            builder.ToTable("DisasterTimelineEvents");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.EventType)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Description)
                .HasMaxLength(1000)
                .IsRequired();

            builder.Property(x => x.OccurredAt)
                .IsRequired();

            builder.HasIndex(x => x.DisasterId);

            builder.HasOne<Disaster>()
                .WithMany()
                .HasForeignKey(x => x.DisasterId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
