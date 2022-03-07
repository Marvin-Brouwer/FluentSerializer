using System.Collections.Generic;

namespace FluentSerializer.UseCase.Mavenlink.Models.Entities;

internal sealed class User : IMavenlinkEntity
{
	public string Id { get; init; } = string.Empty;
	public string Name { get; init; } = string.Empty;
	public int Age { get; init; }

	public string? AccountMembershipId { get; init; }
	public AccountMembership? AccountMembership { get; init; }

	/// <summary>
	/// Because of the custom fields allowing different types of values you don't know up front,
	/// you'll have to use a value of <see cref="IJsonValue"/>. <br />
	/// Therefore, it's provably better to specifically target custom field values. <br />
	/// Chances exist you know what you're looking for up front anyway. <br />
	/// <br />
	/// However, Since this API supports more than just references to custom fields,
	/// having a generic converter for these things still can be useful.
	/// </summary>
	public List<CustomFieldValue>? CustomFields { get; init; }

	public string? Nickname { get; init; }
}