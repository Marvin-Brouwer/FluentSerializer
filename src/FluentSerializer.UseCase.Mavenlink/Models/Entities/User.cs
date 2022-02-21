namespace FluentSerializer.UseCase.Mavenlink.Models.Entities;

internal sealed class User : IMavenlinkEntity
{
	public string Id { get; init; } = string.Empty;
	public string Name { get; init; } = string.Empty;
	public int Age { get; init; } = default!;

	public string? AccountMembershipId { get; set; }

	// todo referenceConverter
	public AccountMembership? AccountMembership { get; set; }

	// todo custom fields with reference converter
}