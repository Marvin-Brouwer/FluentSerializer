using FluentSerializer.Core.Configuration;
using FluentSerializer.Json.Converting;
using FluentSerializer.Json.Profiles;
using FluentSerializer.UseCase.Mavenlink.Models;
using FluentSerializer.UseCase.Mavenlink.Serializer.Converters;

namespace FluentSerializer.UseCase.Mavenlink.Serializer.Profiles
{
    public sealed class UserProfile : JsonSerializerProfile
    {
        protected override void Configure()
		{
			For<User>(
					direction: SerializerDirection.Deserialize
				)
				.Property(user => user.Id)
				.Property(user => user.Name)
				.Property(user => user.Age)
				.Property(user => user.AccountMembershipId)
				.Property(user => user.AccountMembershipId,
				converter: Converter.For.ReferenceTo<User>(user => user.AccountMembership));

			For<AccountMembership>(
					direction: SerializerDirection.Deserialize
				)
				.Property(account => account.Id)
				.Property(account => account.LineManagerId)
				.Property(account => account.LineManagerId,
					converter: Converter.For.ReferenceTo<AccountMembership>(account => account.LineManager));
		}
    }
}
