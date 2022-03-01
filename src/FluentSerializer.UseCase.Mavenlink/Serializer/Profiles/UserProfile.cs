using FluentSerializer.Core.Configuration;
using FluentSerializer.Json.Profiles;
using FluentSerializer.UseCase.Mavenlink.Models;

namespace FluentSerializer.UseCase.Mavenlink.Serializer.Profiles;

public sealed class UserProfile : JsonSerializerProfile
{
	protected override void Configure()
	{
		For<User>(
				direction: SerializerDirection.Deserialize
			)
			.Property(user => user.Id)
			.Property(user => user.Name)
			.Property(user => user.Age);
	}
}