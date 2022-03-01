using FluentSerializer.Core.Naming;
using FluentSerializer.UseCase.OpenAir.Models.Base;
using FluentSerializer.UseCase.OpenAir.Models.Response;
using FluentSerializer.UseCase.OpenAir.Serializer.NamingStrategies;
using FluentSerializer.Xml.Converting;
using FluentSerializer.Xml.Profiles;

namespace FluentSerializer.UseCase.OpenAir.Serializer.Profiles.Base;

/// <remarks>
/// This is a temporary setup, in the final implementation the For{T} method will have an overload that supports base types.
/// </remarks>
public static class ResponseBaseProfile
{
	internal static IXmlProfileBuilder<TResponseObject> UseBase<TResponseObject>(this IXmlProfileBuilder<TResponseObject> builder)
		where TResponseObject : ResponseObject<OpenAirEntity>, new()
	{
		builder 
			.Attribute(responseObject => responseObject.StatusCode,
				namingStrategy: Names.Equal("status"))
			.Child(response => response.Data,
				namingStrategy: Names.Use.ResponseTypeName,
				converter: Converter.For.Collection(false));

		return builder;
	}

}