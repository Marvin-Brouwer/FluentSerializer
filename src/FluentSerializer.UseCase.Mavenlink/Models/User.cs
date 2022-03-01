namespace FluentSerializer.UseCase.Mavenlink.Models;

internal sealed class User : IMavenlinkEntity
{
	public string Id { get; init; } = string.Empty;
	public string Name { get; init; } = string.Empty;
	public int Age { get; init; } = default!;

	// todo accountmembership with reference converter
	// todo custom fields with reference converter
}