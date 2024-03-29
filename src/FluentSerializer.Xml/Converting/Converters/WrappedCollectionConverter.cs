using FluentSerializer.Core.Context;
using FluentSerializer.Core.Converting.Converters;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Xml.DataNodes;
using FluentSerializer.Xml.Services;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using static FluentSerializer.Xml.XmlBuilder;

namespace FluentSerializer.Xml.Converting.Converters;

/// <summary>
/// Converts most dotnet collections and DOES wrap in a property name tag
/// <code>
/// <![CDATA[
/// class Example {
///	  public IEnumerabble<ExampleProp> Prop { get; set; }
/// }
/// ]]>
/// </code>
/// <code>
/// <![CDATA[
/// <Class>
///   <propertyName>
///		<ItemClass />
///		<ItemClass />
///	  </propertyName>
/// </Class>
/// ]]>
/// </code>
/// </summary>
public class WrappedCollectionConverter : CollectionConverterBase, IXmlConverter<IXmlElement>
{
	/// <inheritdoc />
	public object? Deserialize(in IXmlElement objectToDeserialize, in ISerializerContext<IXmlNode> context)
	{
		var targetType = context.PropertyType;
		var collection = GetEnumerableInstance(in targetType);

		var genericTargetType = context.PropertyType.IsGenericType
			? context.PropertyType.GetTypeInfo().GenericTypeArguments[0]
			: collection.GetEnumerator().Current?.GetType() ?? typeof(object);

		var itemNamingStrategy = context.FindNamingStrategy(in genericTargetType) ?? context.NamingStrategy;

		var itemName = itemNamingStrategy.SafeGetName(genericTargetType, context);
		var elementsToDeserialize = objectToDeserialize.GetChildElements(itemName);

		foreach (var item in elementsToDeserialize)
		{
			var itemValue = ((IAdvancedXmlSerializer)context.CurrentSerializer).Deserialize(item, genericTargetType, context);
			if (itemValue is null) continue;

			collection.Add(itemValue);
		}

		return FinalizeEnumerableInstance(in collection, in targetType);
	}

	/// <inheritdoc />
	public IXmlElement? Serialize(in object objectToSerialize, in ISerializerContext context)
	{
		if (objectToSerialize is not IEnumerable enumerableToSerialize)
			throw new NotSupportedException($"Type '{objectToSerialize.GetType().FullName}' does not implement IEnumerable");

		var elementName = context.NamingStrategy.SafeGetName(context.Property, context.PropertyType, context);

		var elements = GetArrayElements((IAdvancedXmlSerializer)context.CurrentSerializer, enumerableToSerialize);
		return Element(in elementName, elements);
	}

	private static IEnumerable<IXmlElement> GetArrayElements(IAdvancedXmlSerializer serializer, IEnumerable enumerableToSerialize)
	{
		foreach (var collectionItem in enumerableToSerialize)
		{
			if (collectionItem is null) continue;
			var itemValue = serializer.SerializeToElement(collectionItem, collectionItem.GetType());
			if (itemValue is null) continue;

			yield return itemValue;
		}
	}
}