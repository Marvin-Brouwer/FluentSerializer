using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Converting.Converters;
using FluentSerializer.Core.Services;
using FluentSerializer.Json.DataNodes;
using FluentSerializer.Json.Services;

using static FluentSerializer.Json.JsonBuilder;

namespace FluentSerializer.Json.Converting.Converters;

/// <summary>
/// Converts most dotnet collections
/// </summary>
public class CollectionConverter : CollectionConverterBase, IJsonConverter
{
	/// <inheritdoc />
	public virtual object? Deserialize(in IJsonNode objectToDeserialize, in ISerializerContext<IJsonNode> context)
	{
		if (objectToDeserialize is not IJsonArray arrayToDeserialize)
			throw new NotSupportedException($"The json object you attempted to deserialize was not a collection");

		return DeserializeCollection(in arrayToDeserialize, context.PropertyType, context.CurrentSerializer);
	}

	/// <summary>
	/// Convert an <see cref="IJsonNode"/> to an enumerable type
	/// </summary>
	/// <exception cref="NotSupportedException"></exception>
	public static object? DeserializeCollection(in IJsonArray arrayToDeserialize, in Type targetType, in ISerializer jsonSerializer)
	{
		var collection = GetEnumerableInstance(in targetType);

		var genericTargetType = targetType.IsGenericType
			? targetType.GetTypeInfo().GenericTypeArguments[0]
			: collection.GetEnumerator().Current?.GetType() ?? typeof(object);

		foreach (var item in arrayToDeserialize.Children)
		{
			// This will skip comments
			if (item is not IJsonContainer container) continue;

			var itemValue = ((IAdvancedJsonSerializer)jsonSerializer).Deserialize(container, genericTargetType);
			if (itemValue is null) continue;

			collection.Add(itemValue);
		}

		return FinalizeEnumerableInstance(in collection, in targetType);
	}

	/// <inheritdoc />
	public virtual IJsonNode Serialize(in object objectToSerialize, in ISerializerContext context)
	{
		if (objectToSerialize is not IEnumerable enumerableToSerialize)
			throw new NotSupportedException($"Type '{objectToSerialize.GetType().FullName}' does not implement IEnumerable");

		return SerializeCollection(in enumerableToSerialize, context.CurrentSerializer);
	}

	/// <summary>
	/// Convert a collection into a JSON array
	/// </summary>
	public static IJsonArray SerializeCollection(in IEnumerable enumerableToSerialize, in ISerializer jsonSerializer)
	{
		var elements = GetArrayItems((IAdvancedJsonSerializer)jsonSerializer, enumerableToSerialize);
		return Array(in elements);
	}

	private static IEnumerable<IJsonArrayContent> GetArrayItems(IAdvancedJsonSerializer serializer, IEnumerable enumerableToSerialize)
	{
		foreach (var collectionItem in enumerableToSerialize)
		{
			if (collectionItem is null) continue;
			var itemValue = serializer.SerializeToContainer<IJsonContainer>(in collectionItem, collectionItem.GetType());
			if (itemValue is not IJsonArrayContent arrayItem) continue;

			yield return arrayItem;
		}
	}
}