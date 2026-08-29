using Eghatha.Domain.Notifications;
using Eghatha.Tests.Common.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Domain.UnitTests.Notifications
{
    public class NotificationTests
    {
        // ---------- Create ----------

        [Fact]
        public void Create_WithValidData_ReturnsNotificationWithExpectedValues()
        {
            var result = NotificationBuilder.Valid()
                .WithTitle("Disaster Reported")
                .WithMessage("A new disaster has been reported near you.")
                .WithUrl("https://example.com/disasters/123")
                .WithType(NotificationType.DisasterReported)
                .Build();

            Assert.False(result.IsError);
            var notification = result.Value;
            Assert.NotEqual(Guid.Empty, notification.Id);
            Assert.Equal("Disaster Reported", notification.Title);
            Assert.Equal("A new disaster has been reported near you.", notification.Message);
            Assert.Equal("https://example.com/disasters/123", notification.Url);
            Assert.Equal(NotificationType.DisasterReported, notification.Type);
            Assert.Empty(notification.Recipients);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Create_WithMissingTitle_ReturnsInvalidTitleError(string? title)
        {
            var result = NotificationBuilder.Valid().WithTitle(title!).Build();

            Assert.True(result.IsError);
            Assert.Equal(NotificationErrors.InvalidTitle, result.FirstError);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Create_WithMissingMessage_ReturnsInvalidMessageError(string? message)
        {
            var result = NotificationBuilder.Valid().WithMessage(message!).Build();

            Assert.True(result.IsError);
            Assert.Equal(NotificationErrors.InvalidMessage, result.FirstError);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Create_WithMissingUrl_ReturnsInvalidUrlError(string? url)
        {
            var result = NotificationBuilder.Valid().WithUrl(url!).Build();

            Assert.True(result.IsError);
            Assert.Equal(NotificationErrors.InvalidUrl, result.FirstError);
        }

        // ---------- AddRecipient ----------

        [Fact]
        public void AddRecipient_WithValidUserId_AddsRecipientToCollection()
        {
            var notification = NotificationTestFactory.CreateValid();
            var userId = Guid.NewGuid();

            notification.AddRecipient(userId);

            var recipient = Assert.Single(notification.Recipients);
            Assert.Equal(userId, recipient.UserId);
            Assert.Equal(notification.Id, recipient.NotificationId);
            Assert.False(recipient.IsRead);
            Assert.Null(recipient.ReadAt);
        }

        [Fact]
        public void AddRecipient_CalledMultipleTimes_AddsEachAsSeparateRecipient()
        {
            var notification = NotificationTestFactory.CreateValid();
            var firstUserId = Guid.NewGuid();
            var secondUserId = Guid.NewGuid();

            notification.AddRecipient(firstUserId);
            notification.AddRecipient(secondUserId);

            Assert.Equal(2, notification.Recipients.Count);
            Assert.Contains(notification.Recipients, r => r.UserId == firstUserId);
            Assert.Contains(notification.Recipients, r => r.UserId == secondUserId);
        }

      
    }
}
