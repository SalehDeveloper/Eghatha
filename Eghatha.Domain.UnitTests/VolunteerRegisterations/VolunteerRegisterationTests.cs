using Eghatha.Domain.Shared.Errors;
using Eghatha.Domain.VolunteerRegisterations;
using Eghatha.Domain.VolunteerRegisterations.Events;
using Eghatha.Domain.Volunteers;
using Eghatha.Tests.Common.VolunteerRegisterations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Domain.UnitTests.VolunteerRegisterations
{
    public class VolunteerRegisterationTests
    {
        // ---------- Create ----------

        [Fact]
        public void Create_WithValidData_ReturnsPendingRegisterationWithExpectedDefaults()
        {
            var volunteerId = Guid.NewGuid();
            var requestedAt = new DateTimeOffset(2026, 3, 1, 9, 0, 0, TimeSpan.Zero);

            var result = VolunteerRegisterationBuilder.Valid()
                .WithVolunteerId(volunteerId)
                .WithRequestedAt(requestedAt)
                .Build();

            Assert.False(result.IsError);
            var registeration = result.Value;
            Assert.NotEqual(Guid.Empty, registeration.Id);
            Assert.Equal(volunteerId, registeration.VolunteerId);
            Assert.Equal(requestedAt, registeration.RequestedAt);
            Assert.Equal(RegisterationStatus.Pending, registeration.Status);
            Assert.Null(registeration.ReviewedAt);
            Assert.Null(registeration.ReviewedByAdminId);
            Assert.Null(registeration.RejectionReason);
        }

        [Fact]
        public void Create_WithValidData_RaisesVolunteerRegisterationCreatedEventWithCorrectPayload()
        {
            var volunteerId = Guid.NewGuid();
            var requestedAt = new DateTimeOffset(2026, 3, 1, 9, 0, 0, TimeSpan.Zero);

            var result = VolunteerRegisterationBuilder.Valid()
                .WithVolunteerId(volunteerId)
                .WithRequestedAt(requestedAt)
                .Build();

            Assert.False(result.IsError);
            var registeration = result.Value;
            var raised = Assert.Single(registeration.DomainEvents.OfType<VolunteerRegisterationCreated>());
            Assert.Equal(registeration.Id, raised.RegisterationId);
            Assert.Equal(volunteerId, raised.VolunteerId);
            Assert.Equal(requestedAt, raised.CreatedAt);
        }

        [Fact]
        public void Create_WithEmptyVolunteerId_ReturnsIdMustBeProvidedError()
        {
            var result = VolunteerRegisterationBuilder.Valid().WithVolunteerId(Guid.Empty).Build();

            Assert.True(result.IsError);
            Assert.Equal(DomainErrors.IdMustBeProvided(nameof(Volunteer)), result.FirstError);
        }

        // ---------- Approve ----------

        [Fact]
        public void Approve_WhenPending_SetsApprovedStatusAndReviewFields()
        {
            var registeration = VolunteerRegisterationTestFactory.CreatePending();
            var reviewedAt = new DateTimeOffset(2026, 3, 2, 10, 0, 0, TimeSpan.Zero);
            var reviewedById = Guid.NewGuid();

            var result = registeration.Approve(reviewedAt, reviewedById);

            Assert.False(result.IsError);
            Assert.Equal(RegisterationStatus.Approved, registeration.Status);
            Assert.Equal(reviewedAt, registeration.ReviewedAt);
            Assert.Equal(reviewedById, registeration.ReviewedByAdminId);
            Assert.Null(registeration.RejectionReason);
        }

        [Fact]
        public void Approve_WhenPending_RaisesVolunteerRegisterationApprovedEventWithCorrectPayload()
        {
            var registeration = VolunteerRegisterationTestFactory.CreatePending();

            var result = registeration.Approve(DateTimeOffset.UtcNow, Guid.NewGuid());

            Assert.False(result.IsError);
            var raised = Assert.Single(registeration.DomainEvents.OfType<VolunteerRegisterationApproved>());
            Assert.Equal(registeration.VolunteerId, raised.VolunteerId);
        }

        [Fact]
        public void Approve_WhenAlreadyApproved_ReturnsAlreadyProcessedErrorAndLeavesFieldsUnchanged()
        {
            var reviewedAt = new DateTimeOffset(2026, 3, 2, 10, 0, 0, TimeSpan.Zero);
            var reviewedById = Guid.NewGuid();
            var registeration = VolunteerRegisterationTestFactory.CreateApproved(reviewedAt, reviewedById);

            var result = registeration.Approve(DateTimeOffset.UtcNow, Guid.NewGuid());

            Assert.True(result.IsError);
            Assert.Equal(VolunteerRegisterationErrors.AlreadyProcessed, result.FirstError);
            Assert.Equal(RegisterationStatus.Approved, registeration.Status);
            Assert.Equal(reviewedAt, registeration.ReviewedAt);
            Assert.Equal(reviewedById, registeration.ReviewedByAdminId);
        }

        [Fact]
        public void Approve_WhenAlreadyRejected_ReturnsAlreadyProcessedErrorAndLeavesFieldsUnchanged()
        {
            var reviewedAt = new DateTimeOffset(2026, 3, 2, 10, 0, 0, TimeSpan.Zero);
            var reviewedById = Guid.NewGuid();
            var registeration = VolunteerRegisterationTestFactory.CreateRejected(reviewedAt, reviewedById, "Not qualified");

            var result = registeration.Approve(DateTimeOffset.UtcNow, Guid.NewGuid());

            Assert.True(result.IsError);
            Assert.Equal(VolunteerRegisterationErrors.AlreadyProcessed, result.FirstError);
            Assert.Equal(RegisterationStatus.Rejected, registeration.Status);
            Assert.Equal(reviewedAt, registeration.ReviewedAt);
            Assert.Equal(reviewedById, registeration.ReviewedByAdminId);
            Assert.Equal("Not qualified", registeration.RejectionReason);
        }

        // ---------- Reject ----------

        [Fact]
        public void Reject_WhenPending_SetsRejectedStatusAndReviewFields()
        {
            var registeration = VolunteerRegisterationTestFactory.CreatePending();
            var reviewedAt = new DateTimeOffset(2026, 3, 2, 10, 0, 0, TimeSpan.Zero);
            var reviewedById = Guid.NewGuid();

            var result = registeration.Reject(reviewedAt, reviewedById, "Missing certifications");

            Assert.False(result.IsError);
            Assert.Equal(RegisterationStatus.Rejected, registeration.Status);
            Assert.Equal(reviewedAt, registeration.ReviewedAt);
            Assert.Equal(reviewedById, registeration.ReviewedByAdminId);
            Assert.Equal("Missing certifications", registeration.RejectionReason);
        }

        [Fact]
        public void Reject_WhenPending_RaisesVolunteerRegisterationRejectedEventWithCorrectPayload()
        {
            var registeration = VolunteerRegisterationTestFactory.CreatePending();

            var result = registeration.Reject(DateTimeOffset.UtcNow, Guid.NewGuid(), "Missing certifications");

            Assert.False(result.IsError);
            var raised = Assert.Single(registeration.DomainEvents.OfType<VolunteerRegisterationRejected>());
            Assert.Equal(registeration.VolunteerId, raised.VolunteerId);
            Assert.Equal("Missing certifications", raised.Reason);
        }

        [Fact]
        public void Reject_WhenAlreadyApproved_ReturnsAlreadyProcessedErrorAndLeavesFieldsUnchanged()
        {
            var reviewedAt = new DateTimeOffset(2026, 3, 2, 10, 0, 0, TimeSpan.Zero);
            var reviewedById = Guid.NewGuid();
            var registeration = VolunteerRegisterationTestFactory.CreateApproved(reviewedAt, reviewedById);

            var result = registeration.Reject(DateTimeOffset.UtcNow, Guid.NewGuid(), "Too late");

            Assert.True(result.IsError);
            Assert.Equal(VolunteerRegisterationErrors.AlreadyProcessed, result.FirstError);
            Assert.Equal(RegisterationStatus.Approved, registeration.Status);
            Assert.Equal(reviewedAt, registeration.ReviewedAt);
            Assert.Equal(reviewedById, registeration.ReviewedByAdminId);
            Assert.Null(registeration.RejectionReason);
        }

        [Fact]
        public void Reject_WhenAlreadyRejected_ReturnsAlreadyProcessedErrorAndLeavesFieldsUnchanged()
        {
            var reviewedAt = new DateTimeOffset(2026, 3, 2, 10, 0, 0, TimeSpan.Zero);
            var reviewedById = Guid.NewGuid();
            var registeration = VolunteerRegisterationTestFactory.CreateRejected(reviewedAt, reviewedById, "Not qualified");

            var result = registeration.Reject(DateTimeOffset.UtcNow, Guid.NewGuid(), "Too late");

            Assert.True(result.IsError);
            Assert.Equal(VolunteerRegisterationErrors.AlreadyProcessed, result.FirstError);
            Assert.Equal(RegisterationStatus.Rejected, registeration.Status);
            Assert.Equal(reviewedAt, registeration.ReviewedAt);
            Assert.Equal(reviewedById, registeration.ReviewedByAdminId);
            Assert.Equal("Not qualified", registeration.RejectionReason);
        }
    }
}
