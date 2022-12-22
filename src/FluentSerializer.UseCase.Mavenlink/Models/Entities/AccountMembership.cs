namespace FluentSerializer.UseCase.Mavenlink.Models.Entities;

internal sealed class AccountMembership : IMavenlinkEntity
{
	public string Id { get; init; } = string.Empty;

	public string? LineManagerId { get; set; }

	public User? LineManager { get; set; }
}