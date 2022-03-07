using System;
using System.Collections.Generic;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Converting.Converters;
using FluentSerializer.Json.Converting;
using FluentSerializer.Json.DataNodes;
using FluentSerializer.UseCase.Mavenlink.Models;
using FluentSerializer.UseCase.Mavenlink.Models.Entities;

namespace FluentSerializer.UseCase.Mavenlink.Serializer.Converters;

/// <summary>
/// A custom converter since Mavenlink returns a list of references to an include for nested data structures.
/// </summary>
/// <example>
/// <![CDATA[
/// {
///     "users": {
///         "9770395": {
///             "custom_field_value_ids": [ "some_field_1", ... ]
///         },
///         "9770335": {
///             ....
///         }
///     ],
///     "custom_field_values": {
///			"some_field_1": {
///             ....
///         }
///     }
/// }
/// ]]>
/// </example>
internal sealed class MavenlinkIdListReferenceConverter : CollectionConverterBase, IJsonConverter
{
	/// <inheritdoc cref="CollectionConverterBase.Direction" />
	public override SerializerDirection Direction => SerializerDirection.Deserialize;

	/// <inheritdoc cref="CollectionConverterBase.Direction" />
	public override bool CanConvert(in Type targetType) => typeof(IEnumerable<IMavenlinkEntity>).IsAssignableFrom(targetType);

	public IJsonNode? Serialize(in object objectToSerialize, in ISerializerContext context) =>
		throw new NotSupportedException();

	public object? Deserialize(in IJsonNode objectToDeserialize, in ISerializerContext<IJsonNode> context)
	{
		if (objectToDeserialize is not IJsonArray jsonArrayToDeserialize) throw new NotSupportedException();

		var referenceCollection = GetEnumerableInstance(context.PropertyType);
		var itemType = context.PropertyType.GetGenericArguments()[0];
		var targetNodeCollectionName = EntityMappings.GetDataGroupName(itemType.Name);

		foreach (var child in jsonArrayToDeserialize.Children)
		{
			if (child is not IJsonValue value) continue;
			var item = MavenlinkIdReferenceConverter.DeserializeReference(in value, in targetNodeCollectionName, in itemType, in context);
			if (item is null) continue;

			referenceCollection.Add(item);
		}

		return FinalizeEnumerableInstance(in referenceCollection, context.PropertyType);
	}
}