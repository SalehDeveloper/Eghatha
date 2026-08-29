using Eghatha.Domain.Shared.Errors;
using Eghatha.Tests.Common.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Domain.UnitTests.Notifications
{
    public class NotificationRecipientTests
    {
        // ---------- Create ----------

        [Fact]
        public void Create_WithValidData_ReturnsUnreadRecipient()
        {
            var notificationId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            var result = NotificationRecipientBuilder.Valid()
                .WithNotificationId(notificationId)
                .WithUserId(userId)
                .Build();

            Assert.False(result.IsError);
            var recipient = result.Value;
            Assert.NotEqual(Guid.Empty, recipient.Id);
            Assert.Equal(notificationId, recipient.NotificationId);
            Assert.Equal(userId, recipient.UserId);
            Assert.False(recipient.IsRead);
            Assert.Null(recipient.ReadAt);
        }

        [Fact]
        public void Create_WithEmptyNotificationId_ReturnsIdMustBeProvidedError()
        {
            var result = NotificationRecipientBuilder.Valid().WithNotificationId(Guid.Empty).Build();

            Assert.True(result.IsError);
            Assert.Equal(DomainErrors.IdMustBeProvided("Notification"), result.FirstError);
        }

        [Fact]
        public void Create_WithEmptyUserId_ReturnsIdMustBeProvidedError()
        {
            var result = NotificationRecipientBuilder.Valid().WithUserId(Guid.Empty).Build();

            Assert.True(result.IsError);
            Assert.Equal(DomainErrors.IdMustBeProvided("User"), result.FirstError);
        }

        // ---------- MarkAsRead ----------

        [Fact]
        public void MarkAsRead_WhenUnread_SetsIsReadAndReadAt()
        {
            var recipient = NotificationRecipientBuilder.Valid().BuildValid();
            var readAt = new DateTimeOffset(2026, 3, 1, 9, 0, 0, TimeSpan.Zero);

            recipient.MarkAsRead(readAt);

            Assert.True(recipient.IsRead);
            Assert.Equal(readAt, recipient.ReadAt);
        }

        [Fact]
        public void MarkAsRead_WhenAlreadyRead_IsIdempotentAndKeepsOriginalReadAt()
        {
            var recipient = NotificationRecipientBuilder.Valid().BuildValid();
            var firstReadAt = new DateTimeOffset(2026, 3, 1, 9, 0, 0, TimeSpan.Zero);
            var secondReadAt = new DateTimeOffset(2026, 3, 5, 9, 0, 0, TimeSpan.Zero);
            recipient.MarkAsRead(firstReadAt);

            recipient.MarkAsRead(secondReadAt);

            Assert.True(recipient.IsRead);
            Assert.Equal(firstReadAt, recipient.ReadAt);
        }
    }
}
