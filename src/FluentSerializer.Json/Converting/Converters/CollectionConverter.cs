using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Json.DataNodes;
using FluentSerializer.Json.Services;

using static FluentSerializer.Json.JsonBuilder;

namespace FluentSerializer.Json.Converting.Converters;

/// <summary>
/// Converts most dotnet collections
/// </summary>
public class CollectionConverter : IJsonConverter
{
	/// <inheritdoc />
	public virtual SerializerDirection Direction { get; } = SerializerDirection.Both;
	/// <inheritdoc />
	public virtual bool CanConvert(in Type targetType) => targetType.IsEnumerable();

	/// <inheritdoc />
	public object? Deserialize(in IJsonNode objectToDeserialize, in ISerializerContext context)
	{
		if (objectToDeserialize is not IJsonArray arrayToDeserialize)
			throw new NotSupportedException($"The json object you attempted to deserialize was not a collection");

		var targetType = context.PropertyType;
		var instance = targetType.GetEnumerableInstance();

		var genericTargetType = context.PropertyType.IsGenericType
			? context.PropertyType.GetTypeInfo().GenericTypeArguments[0]
			: instance.GetEnumerator().Current?.GetType() ?? typeof(object);
            
		foreach (var item in arrayToDeserialize.Children)
		{
			// This will skip comments
			if (item is not IJsonContainer container) continue;

			var itemValue = ((IAdvancedJsonSerializer)context.CurrentSerializer).Deserialize(container, genericTargetType);
			if (itemValue is null) continue;

			instance.Add(itemValue);
		}

		return instance;
	}

	/// <inheritdoc />
	public IJsonNode Serialize(in object objectToSerialize, in ISerializerContext context)
	{
		if (objectToSerialize is not IEnumerable enumerableToSerialize)
			throw new NotSupportedException($"Type '{objectToSerialize.GetType().FullName}' does not implement IEnumerable");

		var elements = GetArrayItems((IAdvancedJsonSerializer)context.CurrentSerializer, enumerableToSerialize);
		return Array(elements);
	}

	private static IEnumerable<IJsonArrayContent> GetArrayItems(IAdvancedJsonSerializer serializer, IEnumerable enumerableToSerialize)
	{
		foreach (var collectionItem in enumerableToSerialize)
		{
			if (collectionItem is null) continue;
			var itemValue = serializer.SerializeToContainer<IJsonContainer>(collectionItem, collectionItem.GetType());
			if (itemValue is not IJsonArrayContent arrayItem) continue;

			yield return arrayItem;
		}
	}
}