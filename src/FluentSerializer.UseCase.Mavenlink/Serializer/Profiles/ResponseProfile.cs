using FluentSerializer.Core.Naming;
using FluentSerializer.Json.Converting;
using FluentSerializer.Json.Profiles;
using FluentSerializer.UseCase.Mavenlink.Models;
using FluentSerializer.UseCase.Mavenlink.Serializer.Converters;

namespace FluentSerializer.UseCase.Mavenlink.Serializer.Profiles;

public sealed class ResponseProfile : JsonSerializerProfile
{
	protected override void Configure()
	{
		For<Response<IMavenlinkEntity>>()
			.Property(project => project.Count)
			.Property(project => project.PageCount,
				namingStrategy: Names.Equal("meta"),
				converter: Converter.For.MavenlinkResponsePageCount
			)
			.Property(project => project.CurrentPage,
				namingStrategy: Names.Equal("meta"),
				converter: Converter.For.MavenlinkResponseCurrentPage
			)
			.Property(project => project.Data,
				namingStrategy: Names.Equal("results"),
				converter: Converter.For.MavenlinkResponseData
			);
	}
}