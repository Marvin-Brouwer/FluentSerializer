using System;
using Ardalis.GuardClauses;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Json.Converting.Converters;
using FluentSerializer.Json.DataNodes;

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
internal sealed class MavenlinkResponseDataConverter : CollectionConverter
{
	/// <inheritdoc />
	public override SerializerDirection Direction => SerializerDirection.Deserialize;
	/// <inheritdoc />
	public override bool CanConvert(in Type targetType) => targetType.IsEnumerable();

	/// <inheritdoc />
	public override IJsonNode Serialize(in object objectToSerialize, in ISerializerContext context) =>
		throw new NotSupportedException();

	/// <inheritdoc />
	public override object? Deserialize(in IJsonNode objectToDeserialize, in ISerializerContext<IJsonNode> context)
	{
		if (objectToDeserialize is not IJsonArray arrayToDeserialize) throw new NotSupportedException();
		if (arrayToDeserialize.Children.Count == 0) return context.PropertyType.GetEnumerableInstance();

		// Get name of current property type from data
		var firstChild = (IJsonObject)arrayToDeserialize.Children[0];

		var collectionKeyProperty = (IJsonValue?)firstChild.GetProperty("key")?.Value;
		var collectionName = collectionKeyProperty?.Value?[1..^1];
		Guard.Against.NullOrEmpty(collectionName, nameof(collectionName));

		// Find nodes from root
		var parent = (IJsonObject)context.ParentNode!;
		var targetCollection = parent.GetProperty(collectionName)?.Value;
		Guard.Against.Null(targetCollection, nameof(targetCollection));

		// We don't care about the order so just return the targetCollection
		return base.Deserialize(in targetCollection, context);
	}
}