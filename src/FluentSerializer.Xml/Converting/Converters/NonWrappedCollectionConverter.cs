using FluentSerializer.Core.Context;
using FluentSerializer.Core.Converting.Converters;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Xml.DataNodes;
using FluentSerializer.Xml.DataNodes.Nodes;
using FluentSerializer.Xml.Services;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace FluentSerializer.Xml.Converting.Converters;

/// <summary>
/// Converts most dotnet collections and does NOT wrap in a property name tag
/// <code>
/// <![CDATA[
/// class Example {
///	  public IEnumerabble<ExampleProp> Prop { get; set; }
/// }
/// ]]>
/// </code>
/// <code>
/// <![CDATA[
/// <Example>
///   <ExampleProp />
///   <ExampleProp />
/// </Example>
/// ]]>
/// </code>
/// </summary>
public class NonWrappedCollectionConverter : CollectionConverterBase, IXmlConverter<IXmlElement>
{
	/// <inheritdoc />
	public object? Deserialize(in IXmlElement objectToDeserialize, in ISerializerContext<IXmlNode> context)
	{
		if (context.ParentNode is null) throw new NotSupportedException("You cannot deserialize a non-wrapped selection at root level");
		if (context.ParentNode is not IXmlElement parent) throw new NotSupportedException("Only 'IXmlElement' nodes are useful parents for this converter");

		var targetType = context.PropertyType;
		var collection = GetEnumerableInstance(in targetType);

		var genericTargetType = context.PropertyType.IsGenericType
			? context.PropertyType.GetTypeInfo().GenericTypeArguments[0]
			: collection.GetEnumerator().Current?.GetType() ?? typeof(object);

		var itemNamingStrategy = context.FindNamingStrategy(in genericTargetType)
								 ?? context.NamingStrategy;

		var itemName = itemNamingStrategy.SafeGetName(genericTargetType, context);
		var elementsToDeserialize = parent.GetChildElements(itemName);
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

		var elements = GetArrayElements((IAdvancedXmlSerializer)context.CurrentSerializer, enumerableToSerialize);
		return new XmlFragment(elements);
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