using Eghatha.Domain.Notifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eghatha.Infastructure.Data.Configurations
{
    public sealed class NotificationRecipientConfiguration
    : IEntityTypeConfiguration<NotificationRecipient>
    {
        public void Configure(EntityTypeBuilder<NotificationRecipient> builder)
        {
            builder.ToTable("NotificationRecipients");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.IsRead)
                .IsRequired();
            builder.HasIndex(x =>
                    new { x.NotificationId, x.UserId })
                    .IsUnique();

            builder.HasOne(x => x.Notification)
                .WithMany(x => x.Recipients)
                .HasForeignKey(x => x.NotificationId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
