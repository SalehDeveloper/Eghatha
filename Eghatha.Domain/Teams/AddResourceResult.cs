using Eghatha.Domain.Teams.TeamResources;

namespace Eghatha.Domain.Teams;

public sealed record AddResourceResult(
Resource Resource,
bool IsNew);