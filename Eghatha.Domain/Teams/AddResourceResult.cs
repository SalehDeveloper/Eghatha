using Eghatha.Domain.Teams.Resources;

namespace Eghatha.Domain.Teams;

public sealed record AddResourceResult(
Resource Resource,
bool IsNew);