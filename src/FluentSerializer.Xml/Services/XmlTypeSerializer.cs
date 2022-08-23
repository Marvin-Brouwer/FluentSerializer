using Ardalis.GuardClauses;

using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.SerializerException;
using FluentSerializer.Xml.DataNodes;
using FluentSerializer.Xml.Exceptions;

using System;
using System.Collections;
using System.Collections.Generic;

using static FluentSerializer.Xml.XmlBuilder;

namespace FluentSerializer.Xml.Services;

/// <summary>
/// Serializer for JSON using profiles
/// </summary>
public sealed class XmlTypeSerializer
{
	private const SerializerDirection TypeSerializerDirection = SerializerDirection.Serialize;

	private readonly IClassMapCollection _classMappings;

	/// <inheritdoc cref="XmlTypeSerializer" />
	public XmlTypeSerializer(in IClassMapCollection classMapCollection)
	{
		Guard.Against.Null(classMapCollection
#if NETSTANDARD2_1
			, nameof(classMapCollection)
#endif
		);

		_classMappings = classMapCollection;
	}

	/// <summary>
	/// Serialize an instance to an <see cref="IXmlElement"/>
	/// </summary>
	/// <exception cref="MalConfiguredRootNodeException"></exception>
	/// <exception cref="ClassMapNotFoundException"></exception>
	public IXmlElement? SerializeToElement(in object dataModel, in Type classType, in ISerializerCoreContext coreContext)
	{
		Guard.Against.Null(dataModel
#if NETSTANDARD2_1
			, nameof(dataModel)
#endif
		);
		Guard.Against.Null(classType
#if NETSTANDARD2_1
			, nameof(classType)
#endif
		);
		Guard.Against.Null(coreContext
#if NETSTANDARD2_1
			, nameof(coreContext)
#endif
		);

		if (typeof(IEnumerable).IsAssignableFrom(classType)) throw new MalConfiguredRootNodeException(in classType);

		var instanceType = dataModel.GetType();
		var classMap =
			_classMappings.GetClassMapFor(instanceType, TypeSerializerDirection) ??
			_classMappings.GetClassMapFor(classType, TypeSerializerDirection);
		if (classMap is null) throw new ClassMapNotFoundException(in classType);

		var currentCoreContext = coreContext.WithPathSegment(classType);
		var elementName = classMap.NamingStrategy.SafeGetName(in classType, new NamingContext(_classMappings));

		currentCoreContext.TryAddReference(dataModel);

		var childNodes = new List<IXmlNode>();
		foreach(var property in classType.GetProperties())
		{
			var propertyMapping = classMap.GetPropertyMapFor(in property, SerializerDirection.Deserialize);
			if (propertyMapping is null) continue;

			var propertyValue = property.GetValue(dataModel);
			if (!coreContext.CurrentSerializer.Configuration.WriteNull && propertyValue is null) continue;

			if (currentCoreContext.ContainsReference(propertyValue))
			{
				var referenceLoopBehavior = currentCoreContext.CurrentSerializer.Configuration.ReferenceLoopBehavior;
				if (referenceLoopBehavior == ReferenceLoopBehavior.Ignore) continue;

				throw new ReferenceLoopException(in instanceType, property.PropertyType, property.Name);
			}

			var serializerContext = new SerializerContext(
				currentCoreContext.WithPathSegment(property),
				propertyMapping.Property, property.PropertyType, in classType, propertyMapping.NamingStrategy,
				classMap.PropertyMapCollection, _classMappings);

			var childNode = SerializeProperty(in propertyValue, in propertyMapping, in serializerContext);
			if (childNode is not null) childNodes.Add(childNode);
		}

		return Element(in elementName, childNodes);
	}

	private IXmlNode? SerializeProperty(
		in object? propertyValue, in IPropertyMap propertyMapping, in SerializerContext serializerContext)
	{
		if (typeof(IXmlText).IsAssignableFrom(propertyMapping.ContainerType))
			return SerializeXmlTextProperty(in propertyValue, in propertyMapping, in serializerContext);

		if (typeof(IXmlAttribute).IsAssignableFrom(propertyMapping.ContainerType))
			return SerializeXmlAttributeProperty(in propertyValue, in propertyMapping, in serializerContext);

		if (typeof(IXmlElement).IsAssignableFrom(propertyMapping.ContainerType))
			return SerializeXmlElementProperty(in propertyValue, in propertyMapping, in serializerContext);

		throw new ContainerNotSupportedException(propertyMapping.ContainerType);
	}

	private static IXmlNode? SerializeXmlTextProperty(in object? propertyValue, in IPropertyMap propertyMapping, in SerializerContext serializerContext)
	{
		if (propertyValue is null && !serializerContext.CurrentSerializer.Configuration.WriteNull) return null;
		if (propertyValue is null && serializerContext.CurrentSerializer.Configuration.WriteNull) return Text(string.Empty);

		var xmlTextValue = SerializeNode<IXmlText>(in propertyValue!, in propertyMapping, in serializerContext);
		if (xmlTextValue is null && !serializerContext.CurrentSerializer.Configuration.WriteNull) return null;
		if (xmlTextValue is null && serializerContext.CurrentSerializer.Configuration.WriteNull) return Text(string.Empty);

		return xmlTextValue;
	}

	private static IXmlNode? SerializeXmlAttributeProperty(in object? propertyValue, in IPropertyMap propertyMapping, in SerializerContext serializerContext)
	{
		if (propertyValue is null && !serializerContext.CurrentSerializer.Configuration.WriteNull) return null;
		var xmlAttributeName = propertyMapping.NamingStrategy.SafeGetName(
			propertyMapping.Property, propertyMapping.ConcretePropertyType, serializerContext);

		if (propertyValue is null && serializerContext.CurrentSerializer.Configuration.WriteNull) return Attribute(xmlAttributeName, string.Empty);
		var xmlAttributeValue = SerializeNode<IXmlAttribute>(in propertyValue!, in propertyMapping, in serializerContext);
		if (xmlAttributeValue is null && !serializerContext.CurrentSerializer.Configuration.WriteNull) return null;
		if (xmlAttributeValue is null && serializerContext.CurrentSerializer.Configuration.WriteNull) return Attribute(xmlAttributeName, string.Empty);

		return xmlAttributeValue;
	}

	private IXmlNode? SerializeXmlElementProperty(in object? propertyValue, in IPropertyMap propertyMapping, in SerializerContext serializerContext)
	{
		var xmlPropertyName = propertyMapping.NamingStrategy.SafeGetName(
			propertyMapping.Property, propertyMapping.ConcretePropertyType, serializerContext);

		if (propertyValue is null && !serializerContext.CurrentSerializer.Configuration.WriteNull) return null;
		if (propertyValue is null && serializerContext.CurrentSerializer.Configuration.WriteNull) return Element(xmlPropertyName);

		var xmlElementValue = SerializeXElement(in propertyValue!, in propertyMapping, in serializerContext, xmlPropertyName);
		if (xmlElementValue is null && !serializerContext.CurrentSerializer.Configuration.WriteNull) return null;
		if (xmlElementValue is null && serializerContext.CurrentSerializer.Configuration.WriteNull) return Element(xmlPropertyName);

		return xmlElementValue;
	}

	private static TNode? SerializeNode<TNode>(
		in object propertyValue, in IPropertyMap propertyMapping, 
		in SerializerContext serializerContext)
		where TNode : IXmlNode
	{
		var matchingConverter = propertyMapping.GetConverter<TNode, IXmlNode>(
			TypeSerializerDirection, serializerContext.CurrentSerializer);
		if (matchingConverter is null) throw new ConverterNotFoundException(
			propertyMapping.Property.PropertyType, propertyMapping.ContainerType, TypeSerializerDirection);
			
		return matchingConverter.Serialize(in propertyValue, serializerContext);
	}

	private IXmlElement? SerializeXElement(in object propertyValue, in IPropertyMap propertyMapping, in SerializerContext serializerContext, string xmlPropertyName)
	{
		var matchingConverter = propertyMapping.GetConverter<IXmlElement, IXmlNode>(
			TypeSerializerDirection, serializerContext.CurrentSerializer);

		if (matchingConverter is not null)
			return matchingConverter.Serialize(in propertyValue, serializerContext);

		var xmlPropertyValue = SerializeToElement(in propertyValue, serializerContext.PropertyType, serializerContext);
		if (xmlPropertyValue is null) return null;

		return Element(xmlPropertyName, xmlPropertyValue);
	}
}