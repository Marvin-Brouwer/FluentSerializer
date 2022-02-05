using FluentSerializer.Json.Converter.DefaultJson.Converting.Converters;
using FluentSerializer.Json.Converting;

namespace FluentSerializer.Json.Converter.DefaultJson.Extensions;

/// <summary>
/// Extensions to allow for <see cref="IUseJsonConverters"/> to point to a <see cref="JsonNodeConverter"/>
/// </summary>
public static class ConverterExtensions
{
	private static readonly IJsonConverter DefaultJsonConverter = new JsonNodeConverter();

	/// <inheritdoc cref="JsonNodeConverter" />
	public static IJsonConverter Json(this IUseJsonConverters _) => DefaultJsonConverter;
}