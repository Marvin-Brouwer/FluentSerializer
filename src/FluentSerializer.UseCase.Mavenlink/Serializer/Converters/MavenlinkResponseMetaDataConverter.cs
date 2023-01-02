using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Json.Converting;
using FluentSerializer.Json.DataNodes;

using System;
using System.Globalization;

namespace FluentSerializer.UseCase.Mavenlink.Serializer.Converters;

/// <summary>
/// Get a specific metadata field for the response object
/// </summary>
internal sealed class MavenlinkResponseMetaDataConverter : IJsonConverter
{
	private readonly string _metaField;

	/// <inheritdoc />
	public SerializerDirection Direction => SerializerDirection.Deserialize;
	/// <inheritdoc />
	public bool CanConvert(in Type targetType) => typeof(int) == targetType;

	/// <inheritdoc />
	public Guid ConverterId { get; } = typeof(MavenlinkResponseDataConverter).GUID;

	/// <inheritdoc cref="MavenlinkResponseMetaDataConverter"/>
	public MavenlinkResponseMetaDataConverter(in string metaField)
	{
		_metaField = metaField;
	}

	public IJsonNode? Serialize(in object objectToSerialize, in ISerializerContext context) =>
		throw new NotSupportedException();

	public object? Deserialize(in IJsonNode objectToDeserialize, in ISerializerContext<IJsonNode> context)
	{
		if (objectToDeserialize is not IJsonObject jsonObjectToDeserialize) throw new NotSupportedException();
		var property = jsonObjectToDeserialize.GetProperty(_metaField);

		var stringValue = (property?.Value as IJsonValue)?.Value;
		if (stringValue is null) return default;

		return int.Parse(stringValue, CultureInfo.CurrentCulture);
	}
}