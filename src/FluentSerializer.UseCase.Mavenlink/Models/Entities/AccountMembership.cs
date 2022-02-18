namespace FluentSerializer.UseCase.Mavenlink.Models
{
    internal sealed class AccountMembership : IMavenlinkEntity
	{
		public string Id { get; init; } = string.Empty;

		public string? LineManagerId { get; set; }

		// todo referenceConverter
		public User? LineManager { get; set; }
	}
}
