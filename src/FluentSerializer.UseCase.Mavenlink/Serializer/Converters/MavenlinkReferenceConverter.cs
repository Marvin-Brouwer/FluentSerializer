using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Json.Converting;
using FluentSerializer.Json.DataNodes;
using FluentSerializer.Json.Services;
using FluentSerializer.UseCase.Mavenlink.Models;

namespace FluentSerializer.UseCase.Mavenlink.Serializer.Converters;

/// <summary>
/// A custom converter since Mavenlink returns a list of id's with a reference to a collection instead of the nested object.
/// </summary>
/// <remarks>
/// Because the <see cref="System.Collections.Generic.List{T}" /> will always contain one type we can just pick the first item in the list 
/// to determine the collection to resolve.
/// </remarks>
/// <example>
/// <![CDATA[
/// {
///     "results": [
///         {
///             "key": "users",
///             "id": "9770395"
///         },
///         {
///             "key": "users",
///             "id": "9770335"
///         }
///     },
///     "users": {
///         "9770395": {
///             ....
///         },
///         "9770335": {
///             ....
///         }
///     ]
/// }
/// ]]>
/// TODO
/// </example>
internal sealed class MavenlinkReferenceConverter : IJsonConverter
{
	/// <inheritdoc />
	public SerializerDirection Direction => SerializerDirection.Deserialize;

	/// <inheritdoc />
	public bool CanConvert(in Type targetType) => true; //todo

	public IJsonNode? Serialize(in object objectToSerialize, in ISerializerContext context) =>
		throw new NotSupportedException();

	public object? Deserialize(in IJsonNode objectToDeserialize, in ISerializerContext<IJsonNode> context)
	{
		if (objectToDeserialize is not IJsonValue jsonValueToDeserialize) throw new NotSupportedException();
			
		var stringValue = jsonValueToDeserialize.Value?[1..^1];
		if (stringValue is null) return default;

		// Prevent multi level reference lookups, this will create weird results
		if (context.Path.Count > 4) return default;

		// Prevent parsing the same node twice
		var hasCalculatedStore = context.Data<MavenlinkReferenceConverter, bool>();
		if (hasCalculatedStore.TryGetValue(stringValue, out bool hasCalculated) && hasCalculated) return default;

		var targetNodeCollectionName = EntityMappings.GetDataGroupName(context.PropertyType.Name);
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
		var deserializedInstance = ((IAdvancedJsonSerializer)context.CurrentSerializer).Deserialize(targetReferenceNode, context.PropertyType, context);
		

		return deserializedInstance;
	}
}