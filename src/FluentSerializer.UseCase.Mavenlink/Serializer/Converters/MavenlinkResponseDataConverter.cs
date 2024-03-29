using Ardalis.GuardClauses;

using FluentSerializer.Core.Context;
using FluentSerializer.Core.Converting.Converters;
using FluentSerializer.Json.Converting;
using FluentSerializer.Json.DataNodes;
using FluentSerializer.Json.Services;

using System;
using System.Linq;
using System.Reflection;

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
/// </example>
internal sealed class MavenlinkResponseDataConverter : CollectionConverterBase, IJsonConverter
{
	/// <inheritdoc />
	public IJsonNode Serialize(in object objectToSerialize, in ISerializerContext context) =>
		throw new NotSupportedException();

	/// <inheritdoc />
	public object? Deserialize(in IJsonNode objectToDeserialize, in ISerializerContext<IJsonNode> context)
	{
		if (objectToDeserialize is not IJsonArray arrayToDeserialize) throw new NotSupportedException();
		if (arrayToDeserialize.Children.Count == 0) return FinalizeEnumerableInstance(
			GetEnumerableInstance(context.PropertyType), context.PropertyType);

		var firstChild = arrayToDeserialize.Children[0] as IJsonObject;
		Guard.Against.Null(firstChild);

		var collectionKeyProperty = (IJsonValue?)firstChild.GetProperty("key")?.Value;
		var collectionName = collectionKeyProperty?.Value?[1..^1];
		Guard.Against.NullOrEmpty(collectionName);

		// Find nodes from root
		var parent = (IJsonObject)context.ParentNode!;
		var targetCollection = parent.GetProperty(collectionName)?.Value as IJsonObject;
		Guard.Against.Null(targetCollection);

		var flattenedCollection = targetCollection.Children
			.Cast<IJsonProperty>()
			.Select(child => child.Value as IJsonObject)
			.Where(child => child is not null);

		var instance = GetEnumerableInstance(context.PropertyType);

		var genericTargetType = context.PropertyType.IsGenericType
			? context.PropertyType.GetTypeInfo().GenericTypeArguments[0]
			: instance.GetEnumerator().Current?.GetType() ?? typeof(object);

		foreach (var item in flattenedCollection)
		{
			// This will skip comments
			if (item is not IJsonContainer container) continue;

			var itemValue = ((IAdvancedJsonSerializer)context.CurrentSerializer).Deserialize(container, genericTargetType, context);
			if (itemValue is null) continue;

			instance.Add(itemValue);
		}

		return FinalizeEnumerableInstance(in instance, context.PropertyType);
	}
}