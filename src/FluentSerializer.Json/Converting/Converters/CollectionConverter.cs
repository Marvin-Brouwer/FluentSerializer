using FluentSerializer.Core.Context;
using FluentSerializer.Core.Converting.Converters;
using FluentSerializer.Json.DataNodes;
using FluentSerializer.Json.Services;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using static FluentSerializer.Json.JsonBuilder;

namespace FluentSerializer.Json.Converting.Converters;

/// <summary>
/// Converts most dotnet collections
/// </summary>
public sealed class CollectionConverter : CollectionConverterBase, IJsonConverter
{
	/// <inheritdoc />
	public object? Deserialize(in IJsonNode objectToDeserialize, in ISerializerContext<IJsonNode> context)
	{
		if (objectToDeserialize is not IJsonArray arrayToDeserialize)
			throw new NotSupportedException($"The json object you attempted to deserialize was not a collection");

		return DeserializeCollection(in arrayToDeserialize, context.PropertyType, context);
	}

	/// <summary>
	/// Convert an <see cref="IJsonNode"/> to an enumerable type
	/// </summary>
	/// <exception cref="NotSupportedException"></exception>
	public static object? DeserializeCollection(in IJsonArray arrayToDeserialize, in Type targetType, in ISerializerCoreContext<IJsonNode> context)
	{
		var collection = GetEnumerableInstance(in targetType);

		var genericTargetType = targetType.IsGenericType
			? targetType.GetTypeInfo().GenericTypeArguments[0]
			: collection.GetEnumerator().Current?.GetType() ?? typeof(object);

		foreach (var item in arrayToDeserialize.Children)
		{
			// This will skip comments
			if (item is not IJsonContainer container) continue;

			var itemValue = ((IAdvancedJsonSerializer)context.CurrentSerializer).Deserialize(container, genericTargetType, in context);
			if (itemValue is null) continue;

			collection.Add(itemValue);
		}

		return FinalizeEnumerableInstance(in collection, in targetType);
	}

	/// <inheritdoc />
	public IJsonNode Serialize(in object objectToSerialize, in ISerializerContext context)
	{
		if (objectToSerialize is not IEnumerable enumerableToSerialize)
			throw new NotSupportedException($"Type '{objectToSerialize.GetType().FullName}' does not implement IEnumerable");

		return SerializeCollection(in enumerableToSerialize, context);
	}

	/// <summary>
	/// Convert a collection into a JSON array
	/// </summary>
	public static IJsonArray SerializeCollection(in IEnumerable enumerableToSerialize, in ISerializerCoreContext context)
	{
		var elements = GetArrayItems((IAdvancedJsonSerializer)context.CurrentSerializer, enumerableToSerialize);
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