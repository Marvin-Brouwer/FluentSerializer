using FluentSerializer.Json.Converter.DefaultJson.Extensions;
using FluentSerializer.Json.Converting;
using FluentSerializer.Json.Profiles;
using FluentSerializer.UseCase.Mavenlink.Models.Entities;

namespace FluentSerializer.UseCase.Mavenlink.Serializer.Profiles;

public sealed class CustomFieldProfile : JsonSerializerProfile
{
	protected override void Configure()
	{
		For<CustomFieldValue>()
			.Property(user => user.Id)
			.Property(user => user.Type)
			.Property(user => user.CustomFieldId)
			.Property(user => user.CustomFieldName)
			.Property(user => user.SubjectType)
			.Property(user => user.SubjectId)
			.Property(user => user.Value,
				converter: Converter.For.Json
			)
			.Property(user => user.DisplayValue);
	}
}