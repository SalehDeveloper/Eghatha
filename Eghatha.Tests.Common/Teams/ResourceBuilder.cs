using Eghatha.Domain.Teams.Resources;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Tests.Common.Teams
{
    /// <summary>
    /// Fluent builder that calls <see cref="Resource.Create"/> directly.
    /// Use this to exercise Resource's own validation branches (e.g. id/type/
    /// quantity checks that Team.AddResource never reaches because it always
    /// passes a fresh id and its own Id as teamId); use TeamTestFactory /
    /// Team.AddResource when a test cares about the resource as part of a
    /// Team aggregate instead.
    /// </summary>
    public sealed class ResourceBuilder
    {
        private Guid _id = Guid.NewGuid();
        private Guid _teamId = Guid.NewGuid();
        private ResourceType _type = ResourceType.FirstAidKit;
        private int _quantity = 10;

        public static ResourceBuilder Valid() => new();

        public ResourceBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }

        public ResourceBuilder WithTeamId(Guid teamId)
        {
            _teamId = teamId;
            return this;
        }

        public ResourceBuilder WithType(ResourceType type)
        {
            _type = type;
            return this;
        }

        public ResourceBuilder WithQuantity(int quantity)
        {
            _quantity = quantity;
            return this;
        }

        public ErrorOr<Resource> Build() =>
            Resource.Create(_id, _teamId, _type, _quantity);

        public Resource BuildValid() => Build().Value;
    }
}
