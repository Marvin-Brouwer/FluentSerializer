using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Naming;
using FluentSerializer.Json.Converting;
using FluentSerializer.Json.Profiles;
using FluentSerializer.UseCase.Mavenlink.Models;
using FluentSerializer.UseCase.Mavenlink.Models.Entities;
using FluentSerializer.UseCase.Mavenlink.Serializer.Converters;
using FluentSerializer.UseCase.Mavenlink.Serializer.NamingStrategies;

namespace FluentSerializer.UseCase.Mavenlink.Serializer.Profiles;

public sealed class UserProfile : JsonSerializerProfile
{
	// todo abstract references in extension
	// todo abstract custom fields in extension
	protected override void Configure()
    {
        For<User>()
	        .Property(user => user.Id)
	        .Property(user => user.Name)
	        .Property(user => user.Age)
	        .Property(user => user.AccountMembershipId)
	        .Property(user => user.AccountMembership,
		        namingStrategy: Names.Use.ReferencePointer,
		        converter: Converter.For.Reference,
		        direction: SerializerDirection.Deserialize
			)
			/// Because of the custom fields allowing different types of values you don't know up front,
			/// you'll have to use a value of <see cref="IJsonValue"/>.
			/// Therefore, it's provably better to specifically target custom field values.
			/// Chances exist you know what you're looking for up front anyway.
			/// 
			/// However, Since this API supports more than just references to custom fields,
			/// having a generic converter for these things still can be useful.
			.Property(user => user.CustomFields,
				namingStrategy: Names.Use.ReferencesPointer,
				converter: Converter.For.References,
				direction: SerializerDirection.Deserialize
			)
			.Property(user => user.Nickname,
				namingStrategy: Names.Equal(EntityMappings.GetDataReferenceGroupName(nameof(CustomFieldValue))),
				converter: Converter.For.CustomFieldReference(Names.Equal("nickname")),
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