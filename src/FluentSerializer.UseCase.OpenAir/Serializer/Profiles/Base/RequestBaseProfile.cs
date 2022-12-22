using FluentSerializer.UseCase.OpenAir.Models.Request;
using FluentSerializer.UseCase.OpenAir.Serializer.Converters;
using FluentSerializer.Xml.Converting;
using FluentSerializer.Xml.Profiles;

namespace FluentSerializer.UseCase.OpenAir.Serializer.Profiles.Base;

/// <remarks>
/// This is a temporary setup, in the final implementation the For{T} method will have an overload that supports base types.
/// </remarks>
public static class RequestBaseProfile
{
	internal static IXmlProfileBuilder<TRequestObject> UseBase<TRequestObject>(this IXmlProfileBuilder<TRequestObject> builder)
		where TRequestObject : RequestObject<object>, new()
	{
		builder
			.Attribute(responseObject => responseObject.Type,
				converter: Converter.For.RequestTypeValue)
			.Child(response => response.Data,
				converter: Converter.For.Collection(false));

		return builder;
	}
}