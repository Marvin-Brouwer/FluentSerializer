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
	// todo abstract references in extension
	protected override void Configure()
    {
        For<User>()
	        .Property(user => user.Id)
	        .Property(user => user.Name)
	        .Property(user => user.Age)
	        .Property(user => user.AccountMembershipId)
	        .Property(user => user.AccountMembership,
				// todo reference namingstrategy
		        namingStrategy: Names.Equal(EntityMappings.GetDataReferenceName(nameof(User.AccountMembership))),
		        converter: Converter.For.Reference,
		        direction: SerializerDirection.Deserialize
			)
			// todo add some custom field data and test it
			.Property(user => user.CustomFields,
		        // todo references namingstrategy
				namingStrategy: Names.Equal(EntityMappings.GetDataReferenceGroupName(nameof(CustomFieldValue))),
		        converter: Converter.For.References,
		        direction: SerializerDirection.Deserialize
	        );

		For<AccountMembership>()
			.Property(account => account.Id)
			.Property(account => account.LineManagerId)
			.Property(account => account.LineManager,
				namingStrategy: Names.Equal(EntityMappings.GetDataReferenceName(nameof(AccountMembership.LineManager))),
				converter: Converter.For.Reference,
				direction: SerializerDirection.Deserialize
			);
	}
}