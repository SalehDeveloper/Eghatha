using Eghatha.Domain.Shared.Errors;
using Eghatha.Domain.Teams;
using Eghatha.Domain.Teams.TeamResources;
using Eghatha.Tests.Common.Teams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Domain.UnitTests.Teams
{
    public class ResourceTests
    {
        // ---------- Create ----------

        [Fact]
        public void Create_WithValidData_ReturnsAvailableResource()
        {
            var teamId = Guid.NewGuid();

            var result = ResourceBuilder.Valid()
                .WithTeamId(teamId)
                .WithType(ResourceType.Ambulance)
                .WithQuantity(2)
                .Build();

            Assert.False(result.IsError);
            var resource = result.Value;
            Assert.Equal(teamId, resource.TeamId);
            Assert.Equal(ResourceType.Ambulance, resource.Type);
            Assert.Equal(2, resource.Quantity);
            Assert.Equal(ResourceStatus.Available, resource.Status);
        }

        [Fact]
        public void Create_WithEmptyId_ReturnsIdMustBeProvidedError()
        {
            var result = ResourceBuilder.Valid().WithId(Guid.Empty).Build();

            Assert.True(result.IsError);
            Assert.Equal(DomainErrors.IdMustBeProvided(nameof(Resource)), result.FirstError);
        }

        [Fact]
        public void Create_WithEmptyTeamId_ReturnsIdMustBeProvidedError()
        {
            var result = ResourceBuilder.Valid().WithTeamId(Guid.Empty).Build();

            Assert.True(result.IsError);
            Assert.Equal(DomainErrors.IdMustBeProvided(nameof(Team)), result.FirstError);
        }

        [Fact]
        public void Create_WithNullType_ReturnsResourceTypeRequiredError()
        {
            var result = ResourceBuilder.Valid().WithType(null!).Build();

            Assert.True(result.IsError);
            Assert.Equal(ResourceErrors.ResourceTypeRequired, result.FirstError);
        }

        [Fact]
        public void Create_WithTypeNotInList_ReturnsInvalidResourceTypeError()
        {
            var fakeType = new ResourceType("FakeType", 999, false);

            var result = ResourceBuilder.Valid().WithType(fakeType).Build();

            Assert.True(result.IsError);
            Assert.Equal(ResourceErrors.InvalidResourceType, result.FirstError);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Create_WithNonPositiveQuantity_ReturnsQuantityError(int quantity)
        {
            var result = ResourceBuilder.Valid().WithQuantity(quantity).Build();

            Assert.True(result.IsError);
            Assert.Equal(ResourceErrors.QuantityShouldBeGreaterThanZero, result.FirstError);
        }

        // ---------- IncreaseQuantity ----------

        [Fact]
        public void IncreaseQuantity_WithPositiveAmount_IncreasesQuantity()
        {
            var resource = ResourceBuilder.Valid().WithQuantity(5).BuildValid();

            var result = resource.IncreaseQuantity(3);

            Assert.False(result.IsError);
            Assert.Equal(8, resource.Quantity);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void IncreaseQuantity_WithNonPositiveAmount_ReturnsQuantityErrorAndLeavesQuantityUnchanged(int amount)
        {
            var resource = ResourceBuilder.Valid().WithQuantity(5).BuildValid();

            var result = resource.IncreaseQuantity(amount);

            Assert.True(result.IsError);
            Assert.Equal(ResourceErrors.QuantityShouldBeGreaterThanZero, result.FirstError);
            Assert.Equal(5, resource.Quantity);
        }

        // ---------- DecreaseQuantity ----------

        [Fact]
        public void DecreaseQuantity_WithValidAmount_DecreasesQuantity()
        {
            var resource = ResourceBuilder.Valid().WithQuantity(5).BuildValid();

            var result = resource.DecreaseQuantity(2);

            Assert.False(result.IsError);
            Assert.Equal(3, resource.Quantity);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void DecreaseQuantity_WithNonPositiveAmount_ReturnsQuantityError(int amount)
        {
            var resource = ResourceBuilder.Valid().WithQuantity(5).BuildValid();

            var result = resource.DecreaseQuantity(amount);

            Assert.True(result.IsError);
            Assert.Equal(ResourceErrors.QuantityShouldBeGreaterThanZero, result.FirstError);
            Assert.Equal(5, resource.Quantity);
        }

        [Fact]
        public void DecreaseQuantity_WithAmountGreaterThanQuantity_ReturnsNotEnoughResourcesError()
        {
            var resource = ResourceBuilder.Valid().WithQuantity(5).BuildValid();

            var result = resource.DecreaseQuantity(10);

            Assert.True(result.IsError);
            Assert.Equal(ResourceErrors.NotEnoughResources, result.FirstError);
            Assert.Equal(5, resource.Quantity);
        }

        // ---------- UpdateStatus ----------

        [Fact]
        public void UpdateStatus_WithValidStatus_UpdatesStatus()
        {
            var resource = ResourceBuilder.Valid().BuildValid();

            var result = resource.UpdateStatus(ResourceStatus.Maintenance);

            Assert.False(result.IsError);
            Assert.Equal(ResourceStatus.Maintenance, resource.Status);
        }

        [Fact]
        public void UpdateStatus_WithNullStatus_ReturnsStatusRequiredError()
        {
            var resource = ResourceBuilder.Valid().BuildValid();

            var result = resource.UpdateStatus(null!);

            Assert.True(result.IsError);
            Assert.Equal(ResourceErrors.StatusRequired, result.FirstError);
            Assert.Equal(ResourceStatus.Available, resource.Status);
        }
    }
}
