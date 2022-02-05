using System;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Json.Converting;
using FluentSerializer.Json.DataNodes;

namespace FluentSerializer.Json.Converter.DefaultJson.Converting.Converters;

/// <summary>
/// Converts any raw <see cref="IJsonNode"/>
/// </summary>
public sealed class JsonNodeConverter : IJsonConverter
{
	/// <inheritdoc />
	public SerializerDirection Direction { get; } = SerializerDirection.Both;
	/// <inheritdoc />
	public bool CanConvert(Type targetType) => typeof(IJsonNode).IsAssignableFrom(targetType);

	/// <inheritdoc />
	public IJsonNode? Serialize(object objectToSerialize, ISerializerContext context)
	{
		if (objectToSerialize is IJsonNode element) return element;

		throw new NotSupportedException(
			$"Type of '${objectToSerialize.GetType().FullName}' could not be converted");
	}

	/// <inheritdoc />
	public object? Deserialize(IJsonNode objectToDeserialize, ISerializerContext context)
	{
		if (context.PropertyType.IsInstanceOfType(objectToDeserialize))
			throw new NotSupportedException(
				$"Type of '${objectToDeserialize.GetType().FullName}' is not assignable to {context.PropertyType}");

		return objectToDeserialize;
	}
}