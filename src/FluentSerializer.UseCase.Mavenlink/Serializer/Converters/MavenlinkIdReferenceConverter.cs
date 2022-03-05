using System;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Json.Converting;
using FluentSerializer.Json.DataNodes;
using FluentSerializer.Json.Services;
using FluentSerializer.UseCase.Mavenlink.Models;
using FluentSerializer.UseCase.Mavenlink.Models.Entities;

namespace FluentSerializer.UseCase.Mavenlink.Serializer.Converters;

/// <summary>
/// A custom converter since Mavenlink returns a reference to an include for nested data structures.
/// </summary>
/// <example>
/// <![CDATA[
/// {
///     "users": {
///         "9770395": {
///             "some_field_id": "some_field_1"
///         },
///         "9770335": {
///             ....
///         }
///     ],
///     "some_fields": {
///			"some_field_1": {
///             ....
///         }
///     }
/// }
/// ]]>
/// </example>
internal sealed class MavenlinkIdReferenceConverter : IJsonConverter
{
	/// <inheritdoc />
	public SerializerDirection Direction => SerializerDirection.Deserialize;

	/// <inheritdoc />
	public bool CanConvert(in Type targetType) => typeof(IMavenlinkEntity).IsAssignableFrom(targetType);

	public IJsonNode? Serialize(in object objectToSerialize, in ISerializerContext context) =>
		throw new NotSupportedException();

	public object? Deserialize(in IJsonNode objectToDeserialize, in ISerializerContext<IJsonNode> context)
	{
		if (objectToDeserialize is not IJsonValue jsonValueToDeserialize) throw new NotSupportedException();

		var targetNodeCollectionName = EntityMappings.GetDataGroupName(context.PropertyType.Name);
		return DeserializeReference(in jsonValueToDeserialize, in targetNodeCollectionName, context.PropertyType, in context);
	}

	public static object? DeserializeReference(in IJsonValue jsonValueToDeserialize, in string targetNodeCollectionName, in Type targetType, in ISerializerContext<IJsonNode> context)
	{
		var stringValue = jsonValueToDeserialize.Value?[1..^1];
		if (stringValue is null) return default;

		// Prevent multi level reference lookups, this will create weird results
		if (context.Path.Count > 4) return default;

		// Prevent parsing the same node twice
		var hasCalculatedStore = context.Data<MavenlinkIdReferenceConverter, bool>();
		if (hasCalculatedStore.TryGetValue(stringValue, out bool hasCalculated) && hasCalculated) return default;

		var targetNodeReferenceCollection = (context.RootNode as IJsonObject)!
			.GetProperty(targetNodeCollectionName)?
			.Value as IJsonObject;

		if (targetNodeReferenceCollection is null)
		{
			hasCalculatedStore.Add(stringValue, true);
			return default;
		}
		var targetReferenceNode = targetNodeReferenceCollection.GetProperty(stringValue)?.Value as IJsonObject;

		if (targetReferenceNode is null)
		{
			hasCalculatedStore.Add(stringValue, true);
			return default;
		}

		hasCalculatedStore.Add(stringValue, true);
		var deserializedInstance = ((IAdvancedJsonSerializer)context.CurrentSerializer).Deserialize(targetReferenceNode, targetType, context);

		return deserializedInstance;
	}
}