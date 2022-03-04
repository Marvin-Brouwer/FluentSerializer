using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Naming;
using FluentSerializer.Json.Converting;
using FluentSerializer.Json.Profiles;
using FluentSerializer.UseCase.Mavenlink.Models;
using FluentSerializer.UseCase.Mavenlink.Models.Entities;
using FluentSerializer.UseCase.Mavenlink.Serializer.Converters;

namespace FluentSerializer.UseCase.Mavenlink.Serializer.Profiles;

public sealed class UserProfile : JsonSerializerProfile
{
    protected override void Configure()
    {
        For<User>()
	        .Property(user => user.Id)
	        .Property(user => user.Name)
	        .Property(user => user.Age)
	        .Property(user => user.AccountMembershipId)
	        .Property(user => user.AccountMembership,
		        namingStrategy: Names.Equal(EntityMappings.GetDataItemName(nameof(User.AccountMembershipId))),
		        converter: Converter.For.Reference,
		        direction: SerializerDirection.Deserialize
	        );

		For<AccountMembership>()
			.Property(account => account.Id)
			.Property(account => account.LineManagerId)
			.Property(account => account.LineManager,
				namingStrategy: Names.Equal(EntityMappings.GetDataItemName(nameof(AccountMembership.LineManagerId))),
				converter: Converter.For.Reference,
				direction: SerializerDirection.Deserialize
			);
	}
}