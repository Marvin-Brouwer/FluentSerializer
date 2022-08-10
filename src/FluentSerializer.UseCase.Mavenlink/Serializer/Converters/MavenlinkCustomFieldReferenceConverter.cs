using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Converting.Converters;
using FluentSerializer.Core.Naming.NamingStrategies;
using FluentSerializer.Json.Converting;
using FluentSerializer.Json.DataNodes;
using FluentSerializer.UseCase.Mavenlink.Models;
using FluentSerializer.UseCase.Mavenlink.Models.Entities;

using System;

namespace FluentSerializer.UseCase.Mavenlink.Serializer.Converters;

/// <summary>
/// A custom converter since Mavenlink returns a list of references to an include for custom fields on several structures.
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
internal sealed class MavenlinkCustomFieldReferenceConverter : IJsonConverter
{
	private readonly INamingStrategy _namingStrategy;
	private static readonly string CustomFieldGroupName = EntityMappings.GetDataReferenceGroupName(nameof(CustomFieldValue));

	/// <inheritdoc cref="CollectionConverterBase.Direction" />
	public SerializerDirection Direction => SerializerDirection.Deserialize;

	/// <inheritdoc cref="CollectionConverterBase.Direction" />
	public bool CanConvert(in Type targetType) => true;

	/// <inheritdoc />
	public int ConverterHashCode { get; } = typeof(MavenlinkCustomFieldReferenceConverter).GetHashCode();

	public MavenlinkCustomFieldReferenceConverter(Func<INamingStrategy> namingStrategy)
	{
		_namingStrategy = namingStrategy();
	}

	public IJsonNode? Serialize(in object objectToSerialize, in ISerializerContext context) =>
		throw new NotSupportedException();

	public object? Deserialize(in IJsonNode objectToDeserialize, in ISerializerContext<IJsonNode> context)
	{
		if (objectToDeserialize is not IJsonArray jsonArrayToDeserialize) throw new NotSupportedException();

		var itemName = _namingStrategy
			.GetName(context.Property, context.PropertyType, context)
			.ToString();

		foreach (var child in jsonArrayToDeserialize.Children)
		{
			if (child is not IJsonValue customFieldReference) continue;
			var fieldId = customFieldReference.Value;
			if (string.IsNullOrEmpty(fieldId)) continue;

			var item = MavenlinkIdReferenceConverter.DeserializeReference(in customFieldReference, in CustomFieldGroupName, typeof(CustomFieldValue), in context);
			if (item is not CustomFieldValue customFieldValue) continue;
			if (string.IsNullOrEmpty(customFieldValue.CustomFieldName)) continue;
			if (!customFieldValue.CustomFieldName.Equals(itemName, StringComparison.Ordinal)) continue;

			if (customFieldValue.Value is not IJsonValue rawValue) return default;
			if (!rawValue.HasValue) return default;

			if (context.PropertyType == typeof(string)) return rawValue.Value![1..^1];
			
			return Convert.ChangeType(customFieldValue.Value, context.PropertyType);
		}

		return default;
	}
}