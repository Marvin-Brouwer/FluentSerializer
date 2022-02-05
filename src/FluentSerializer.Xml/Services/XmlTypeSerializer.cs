using Ardalis.GuardClauses;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.SerializerException;
using System;
using System.Collections;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Xml.Exceptions;
using FluentSerializer.Xml.DataNodes;
using System.Collections.Generic;

using static FluentSerializer.Xml.XmlBuilder;
using FluentSerializer.Xml.Profiles;

namespace FluentSerializer.Xml.Services;

/// <summary>
/// Serializer for JSON using profiles
/// </summary>
public sealed class XmlTypeSerializer
{
	private readonly IClassMapScanList<XmlSerializerProfile> _mappings;

	/// <inheritdoc />
	public XmlTypeSerializer(in IClassMapScanList<XmlSerializerProfile> mappings)
	{
		Guard.Against.Null(mappings, nameof(mappings));

		_mappings = mappings;
	}

	/// <summary>
	/// Serialize an instance to an <see cref="IXmlElement"/>
	/// </summary>
	/// <exception cref="MalConfiguredRootNodeException"></exception>
	/// <exception cref="ClassMapNotFoundException"></exception>
	public IXmlElement? SerializeToElement(in object dataModel, in Type classType, in IXmlSerializer currentSerializer)
	{
		Guard.Against.Null(dataModel, nameof(dataModel));
		Guard.Against.Null(classType, nameof(classType));
		Guard.Against.Null(currentSerializer, nameof(currentSerializer));

		if (typeof(IEnumerable).IsAssignableFrom(classType)) throw new MalConfiguredRootNodeException(in classType);

		var classMap = _mappings.Scan((classType, SerializerDirection.Serialize));
		if (classMap is null) throw new ClassMapNotFoundException(in classType);

		var elementName = classMap.NamingStrategy.SafeGetName(in classType, new NamingContext(_mappings));
		var childNodes = new List<IXmlNode>();
		foreach(var property in classType.GetProperties())
		{
			var propertyMapping = classMap.PropertyMaps.Scan(property);
			if (propertyMapping is null) continue;
			if (propertyMapping.Direction == SerializerDirection.Deserialize) continue;

			var propertyValue = property.GetValue(dataModel);
			if (propertyValue is null) continue;

			var serializerContext = new SerializerContext(
				propertyMapping.Property, in classType, propertyMapping.NamingStrategy, 
				currentSerializer,
				classMap.PropertyMaps, _mappings);

			var childNode = SerializeProperty(in propertyValue, in propertyMapping, in currentSerializer, in serializerContext);
			if (childNode is not null) childNodes.Add(childNode);
		}

		return Element(in elementName, childNodes);
	}

	private IXmlNode? SerializeProperty(
		in object propertyValue, in IPropertyMap propertyMapping,  
		in IXmlSerializer currentSerializer, in SerializerContext serializerContext)
	{
		if (typeof(IXmlText).IsAssignableFrom(propertyMapping.ContainerType))
		{
			return SerializeNode<IXmlText>(in propertyValue, in propertyMapping, in serializerContext);
		}

		if (typeof(IXmlAttribute).IsAssignableFrom(propertyMapping.ContainerType))
		{
			return SerializeNode<IXmlAttribute>(in propertyValue, in propertyMapping, in serializerContext);
		}

		if (typeof(IXmlElement).IsAssignableFrom(propertyMapping.ContainerType))
		{
			return SerializeXElement(in propertyValue, in propertyMapping, in serializerContext, in currentSerializer);
		}

		throw new ContainerNotSupportedException(propertyMapping.ContainerType);
	}

	private static TNode? SerializeNode<TNode>(
		in object propertyValue, in IPropertyMap propertyMapping, 
		in SerializerContext serializerContext)
		where TNode : IXmlNode
	{
		var matchingConverter = propertyMapping.GetConverter<TNode>(
			SerializerDirection.Serialize, serializerContext.CurrentSerializer);
		if (matchingConverter is null) throw new ConverterNotFoundException(
			propertyMapping.Property.PropertyType, propertyMapping.ContainerType, SerializerDirection.Serialize);
			
		return matchingConverter.Serialize(in propertyValue, serializerContext);
	}

	private IXmlElement? SerializeXElement(in object propertyValue, in IPropertyMap propertyMapping,
		in SerializerContext serializerContext, in IXmlSerializer currentSerializer)
	{
		var matchingConverter = propertyMapping.GetConverter<IXmlElement>(
			SerializerDirection.Serialize, serializerContext.CurrentSerializer);

		return matchingConverter is null 
			? SerializeToElement(in propertyValue, serializerContext.PropertyType, currentSerializer) 
			: matchingConverter.Serialize(in propertyValue, serializerContext);
	}
}