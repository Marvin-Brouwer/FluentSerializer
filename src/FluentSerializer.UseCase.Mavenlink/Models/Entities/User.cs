using System.Collections.Generic;

namespace FluentSerializer.UseCase.Mavenlink.Models.Entities;

internal sealed class User : IMavenlinkEntity
{
	public string Id { get; init; } = string.Empty;
	public string Name { get; init; } = string.Empty;
	public int Age { get; init; }

	public string? AccountMembershipId { get; init; }
	public AccountMembership? AccountMembership { get; init; }

	public List<CustomFieldValue>? CustomFields { get; init; }
}