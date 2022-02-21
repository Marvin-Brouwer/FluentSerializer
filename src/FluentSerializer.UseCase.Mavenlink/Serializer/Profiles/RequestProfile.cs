using FluentSerializer.Core.Naming;
using FluentSerializer.Json.Profiles;
using FluentSerializer.UseCase.Mavenlink.Models;
using FluentSerializer.UseCase.Mavenlink.Models.Entities;
using FluentSerializer.UseCase.Mavenlink.Serializer.NamingStrategies;

namespace FluentSerializer.UseCase.Mavenlink.Serializer.Profiles;

public sealed class RequestProfile : JsonSerializerProfile
{
	protected override void Configure()
	{
		For<Request<IMavenlinkEntity>>()
			.Property(project => project.Data,
				namingStrategy: Names.Use.RequestEntityName
			);
	}
}