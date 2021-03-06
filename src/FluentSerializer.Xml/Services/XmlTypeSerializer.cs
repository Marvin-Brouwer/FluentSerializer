using Ardalis.GuardClauses;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.SerializerException;
using FluentSerializer.Xml.Configuration;
using FluentSerializer.Xml.DataNodes;
using FluentSerializer.Xml.Exceptions;
using FluentSerializer.Xml.Profiles;
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
	private readonly IClassMapScanList<XmlSerializerProfile, XmlSerializerConfiguration> _mappings;

	/// <inheritdoc cref="XmlTypeSerializer" />
	public XmlTypeSerializer(in IReadOnlyCollection<IClassMap> mappings)
	{
		Guard.Against.Null(mappings, nameof(mappings));

		_mappings = new ClassMapScanList<XmlSerializerProfile, XmlSerializerConfiguration>(mappings);
	}

	/// <summary>
	/// Serialize an instance to an <see cref="IXmlElement"/>
	/// </summary>
	/// <exception cref="MalConfiguredRootNodeException"></exception>
	/// <exception cref="ClassMapNotFoundException"></exception>
	public IXmlElement? SerializeToElement(in object dataModel, in Type classType, in ISerializerCoreContext coreContext)
	{
		Guard.Against.Null(dataModel, nameof(dataModel));
		Guard.Against.Null(classType, nameof(classType));
		Guard.Against.Null(coreContext, nameof(coreContext));

		if (typeof(IEnumerable).IsAssignableFrom(classType)) throw new MalConfiguredRootNodeException(in classType);

		var instanceType = dataModel.GetType();
		var classMap =
			_mappings.Scan((instanceType, SerializerDirection.Serialize)) ??
			_mappings.Scan((classType, SerializerDirection.Serialize));
		if (classMap is null) throw new ClassMapNotFoundException(in classType);

		var currentCoreContext = coreContext.WithPathSegment(classType);
		var elementName = classMap.NamingStrategy.SafeGetName(in classType, new NamingContext(_mappings));

		currentCoreContext.TryAddReference(dataModel);

		var childNodes = new List<IXmlNode>();
		foreach(var property in classType.GetProperties())
		{
			var propertyMapping = classMap.PropertyMaps.Scan(property);
			if (propertyMapping is null) continue;
			if (propertyMapping.Direction == SerializerDirection.Deserialize) continue;

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
				classMap.PropertyMaps, _mappings);

			var childNode = SerializeProperty(in propertyValue, in propertyMapping, in serializerContext);
			if (childNode is not null) childNodes.Add(childNode);
		}

		return Element(in elementName, childNodes);
	}

	private IXmlNode? SerializeProperty(
		in object? propertyValue, in IPropertyMap propertyMapping, in SerializerContext serializerContext)
	{
		if (typeof(IXmlText).IsAssignableFrom(propertyMapping.ContainerType))
		{
			if (propertyValue is null && !serializerContext.CurrentSerializer.Configuration.WriteNull) return null;
			if (propertyValue is null && serializerContext.CurrentSerializer.Configuration.WriteNull) return Text(string.Empty);

			var xmlTextValue = SerializeNode<IXmlText>(in propertyValue!, in propertyMapping, in serializerContext);
			if (xmlTextValue is null && !serializerContext.CurrentSerializer.Configuration.WriteNull) return null;
			if (xmlTextValue is null && serializerContext.CurrentSerializer.Configuration.WriteNull) return Text(string.Empty);

			return xmlTextValue;
		}

		if (typeof(IXmlAttribute).IsAssignableFrom(propertyMapping.ContainerType))
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

		if (typeof(IXmlElement).IsAssignableFrom(propertyMapping.ContainerType))
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

		throw new ContainerNotSupportedException(propertyMapping.ContainerType);
	}

	private static TNode? SerializeNode<TNode>(
		in object propertyValue, in IPropertyMap propertyMapping, 
		in SerializerContext serializerContext)
		where TNode : IXmlNode
	{
		var matchingConverter = propertyMapping.GetConverter<TNode, IXmlNode>(
			SerializerDirection.Serialize, serializerContext.CurrentSerializer);
		if (matchingConverter is null) throw new ConverterNotFoundException(
			propertyMapping.Property.PropertyType, propertyMapping.ContainerType, SerializerDirection.Serialize);
			
		return matchingConverter.Serialize(in propertyValue, serializerContext);
	}

	private IXmlElement? SerializeXElement(in object propertyValue, in IPropertyMap propertyMapping, in SerializerContext serializerContext, string xmlPropertyName)
	{
		var matchingConverter = propertyMapping.GetConverter<IXmlElement, IXmlNode>(
			SerializerDirection.Serialize, serializerContext.CurrentSerializer);

		if (matchingConverter is not null)
			return matchingConverter.Serialize(in propertyValue, serializerContext);

		var xmlPropertyValue = SerializeToElement(in propertyValue!, serializerContext.PropertyType, serializerContext);
		if (xmlPropertyValue is null) return null;

		return Element(xmlPropertyName, xmlPropertyValue);
	}
}