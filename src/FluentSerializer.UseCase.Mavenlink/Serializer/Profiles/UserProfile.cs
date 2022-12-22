using FluentSerializer.Core.Naming;
using FluentSerializer.Json.Profiles;
using FluentSerializer.UseCase.Mavenlink.Extensions;
using FluentSerializer.UseCase.Mavenlink.Models.Entities;

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
			.PropertyForReference(user => user.AccountMembership)
			// Because of the custom fields allowing different types of values you don't know up front,
			// you'll have to use a value of <see cref="IJsonValue"/>.
			// Therefore, it's provably better to specifically target custom field values.
			// Chances exist you know what you're looking for up front anyway.
			// 
			// However, Since this API supports more than just references to custom fields,
			// having a generic converter for these things still can be useful.
			.PropertyForReferences(user => user.CustomFields)
			.PropertyForCustomField(user => user.Nickname, Names.Equal("nickname"));

		For<AccountMembership>()
			.Property(account => account.Id)
			.Property(account => account.LineManagerId)
			.PropertyForReference(account => account.LineManager);
	}
}