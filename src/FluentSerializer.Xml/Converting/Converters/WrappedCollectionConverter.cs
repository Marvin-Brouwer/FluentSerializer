using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Converting;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Xml.DataNodes;
using FluentSerializer.Xml.Services;

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
/// <Example>
///   <Prop>
///		<ExampleProp />
///		<ExampleProp />
///	  </Prop>
/// </Example>
/// ]]>
/// </code>
/// </summary>
public class WrappedCollectionConverter : IXmlConverter<IXmlElement>
{
	/// <inheritdoc />
	public SerializerDirection Direction { get; } = SerializerDirection.Both;
	/// <inheritdoc />
	public bool CanConvert(in Type targetType) => targetType.IsEnumerable();

	object? IConverter<IXmlElement>.Deserialize(in IXmlElement objectToDeserialize, in ISerializerContext context)
	{
		var targetType = context.PropertyType;
		var instance = targetType.GetEnumerableInstance();

		var genericTargetType = context.PropertyType.IsGenericType
			? context.PropertyType.GetTypeInfo().GenericTypeArguments[0]
			: instance.GetEnumerator().Current?.GetType() ?? typeof(object);

		var itemNamingStrategy = context.FindNamingStrategy(in genericTargetType) ?? context.NamingStrategy;

		var itemName = itemNamingStrategy.SafeGetName(genericTargetType, context);
		var elementsToDeserialize = objectToDeserialize.GetChildElements(itemName);

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

		var elementName = context.NamingStrategy.SafeGetName(context.Property, context);

		var elements = GetArrayElements((IAdvancedXmlSerializer)context.CurrentSerializer, enumerableToSerialize);
		return Element(in elementName, elements) ;
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