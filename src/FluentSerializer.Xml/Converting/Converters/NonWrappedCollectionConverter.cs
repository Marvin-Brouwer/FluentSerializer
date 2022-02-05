using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Converting;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Xml.DataNodes;
using FluentSerializer.Xml.DataNodes.Nodes;
using FluentSerializer.Xml.Services;

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
public class NonWrappedCollectionConverter : IXmlConverter<IXmlElement>
{
	/// <inheritdoc />
	public SerializerDirection Direction { get; } = SerializerDirection.Both;
	/// <inheritdoc />
	public bool CanConvert(in Type targetType) => targetType.IsEnumerable();

	object? IConverter<IXmlElement>.Deserialize(in IXmlElement objectToDeserialize, in ISerializerContext context)
	{
		throw new NotSupportedException();
	}

	object? IXmlConverter<IXmlElement>.Deserialize(in IXmlElement objectToDeserialize, in IXmlElement? parent, in ISerializerContext context)
	{
		if (parent is null) throw new NotSupportedException("You cannot deserialize a non-wrapped selection at root level");

		var targetType = context.PropertyType;
		var instance = targetType.GetEnumerableInstance();

		var genericTargetType = context.PropertyType.IsGenericType
			? context.PropertyType.GetTypeInfo().GenericTypeArguments[0]
			: instance.GetEnumerator().Current?.GetType() ?? typeof(object);

		var itemNamingStrategy = context.FindNamingStrategy(in genericTargetType)
		                         ?? context.NamingStrategy;

		var itemName = itemNamingStrategy.SafeGetName(genericTargetType, context);
		var elementsToDeserialize = parent.GetChildElements(itemName);
		foreach (var item in elementsToDeserialize)
		{
			var itemValue = ((IAdvancedXmlSerializer)context.CurrentSerializer).Deserialize(item, genericTargetType);
			if (itemValue is null) continue;

			instance.Add(itemValue);
		}

		return instance;
	}
	IXmlElement? IConverter<IXmlElement>.Serialize(in object objectToSerialize, in ISerializerContext context)
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