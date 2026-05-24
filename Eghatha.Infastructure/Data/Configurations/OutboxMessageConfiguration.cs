using Eghatha.Infastructure.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eghatha.Infastructure.Data.Configurations
{
    public sealed class OutboxMessageConfiguration
    : IEntityTypeConfiguration<OutboxMessage>
    {
        public void Configure(EntityTypeBuilder<OutboxMessage> builder)
        {
            builder.ToTable("OutboxMessages");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedNever();

            builder.Property(x => x.Type)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.Content)
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            builder.Property(x => x.OccurredOnUtc)
                .IsRequired();

            builder.Property(x => x.ProcessedOnUtc);

            builder.Property(x => x.Error)
                .HasColumnType("nvarchar(max)");

            builder.HasIndex(x => x.ProcessedOnUtc);

            builder.HasIndex(x => x.OccurredOnUtc);
        }
    }
}