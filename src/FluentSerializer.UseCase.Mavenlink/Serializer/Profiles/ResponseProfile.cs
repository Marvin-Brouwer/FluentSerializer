using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Naming;
using FluentSerializer.Json.Converting;
using FluentSerializer.Json.Profiles;
using FluentSerializer.UseCase.Mavenlink.Models;
using FluentSerializer.UseCase.Mavenlink.Models.Entities;
using FluentSerializer.UseCase.Mavenlink.Serializer.Converters;

namespace FluentSerializer.UseCase.Mavenlink.Serializer.Profiles;

public sealed class ResponseProfile : JsonSerializerProfile
{
	protected override void Configure()
	{
		For<Response<IMavenlinkEntity>>(
			direction: SerializerDirection.Deserialize
		)
			.Property(project => project.Count)
			.Property(project => project.PageCount,
				namingStrategy: Names.Are("meta"),
				converter: Converter.For.MavenlinkResponsePageCount
			)
			.Property(project => project.CurrentPage,
				namingStrategy: Names.Are("meta"),
				converter: Converter.For.MavenlinkResponseCurrentPage
			)
			.Property(project => project.Data,
				namingStrategy: Names.Are("results"),
				converter: Converter.For.MavenlinkResponseData
			);
	}
}


//{
//	"count": 1,
//	"meta": {
//		"count": 1,
//		"pageCount": 1,
//		"page_number": 1,
//		"page_size": 200
//	},
//	"results": [
//		{
//			"id": "U1",
//			"key": "users"
//		},
//		{
//	"id": "U2",
//			"key": "users"
//		}
//	],
//	....
//}
