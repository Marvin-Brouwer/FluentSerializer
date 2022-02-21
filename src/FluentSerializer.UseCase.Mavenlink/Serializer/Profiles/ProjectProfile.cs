using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Naming;
using FluentSerializer.Json.Converting;
using FluentSerializer.Json.Profiles;
using FluentSerializer.UseCase.Mavenlink.Models.Entities;

namespace FluentSerializer.UseCase.Mavenlink.Serializer.Profiles;

public sealed class ProjectProfile : JsonSerializerProfile
{
	/// <summary>
	/// The custom field is ignored here,
	/// you'll have to make a custom field request to update/create those values.
	/// </summary>
	protected override void Configure()
	{
		For<Project>(
				direction:SerializerDirection.Serialize
			)
			.Property(project => project.Id)
			.Property(project => project.Name)
			.Property(project => project.LastUpdate,
				namingStrategy: Names.Are("updated_at"),
				converter: Converter.For.DateTime("dd/MM/yyyy HH:mm:ss"));
	}
}