using Ardalis.GuardClauses;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.SerializerException;
using FluentSerializer.Xml.Converting;
using FluentSerializer.Xml.DataNodes;
using FluentSerializer.Xml.Exceptions;
using FluentSerializer.Xml.Profiles;
using System;
using System.Collections;

using static FluentSerializer.Xml.XmlBuilder;

namespace FluentSerializer.Xml.Services;

/// <summary>
/// Deserializer for XML using profiles
/// </summary>
public sealed class XmlTypeDeserializer
{
	private readonly IClassMapScanList<XmlSerializerProfile> _mappings;

	/// <inheritdoc />
	public XmlTypeDeserializer(in IClassMapScanList<XmlSerializerProfile> mappings)
	{
		Guard.Against.Null(mappings, nameof(mappings));

		_mappings = mappings;
	}

	/// <summary>
	/// Deserialize an <see cref="IXmlElement"/> to the requested object type
	/// </summary>
	public TModel? DeserializeFromElement<TModel>(in IXmlElement dataObject, in IXmlSerializer currentSerializer)
		where TModel : new()
	{
		Guard.Against.Null(dataObject, nameof(dataObject));
		Guard.Against.Null(currentSerializer, nameof(currentSerializer));

		var classType = typeof(TModel);
		var deserializedInstance = DeserializeFromElement(in dataObject, in classType, in currentSerializer);
		if (deserializedInstance is null) return default;

		return (TModel)deserializedInstance;
	}

	/// <summary>
	/// Deserialize an <see cref="IXmlElement"/> to the requested object type
	/// </summary>
	public object? DeserializeFromElement(in IXmlElement dataObject, in Type classType, in IXmlSerializer currentSerializer)
	{
		Guard.Against.Null(dataObject, nameof(dataObject));
		Guard.Against.Null(classType, nameof(classType));
		Guard.Against.Null(currentSerializer, nameof(currentSerializer));

		if (typeof(IEnumerable).IsAssignableFrom(classType)) throw new MalConfiguredRootNodeException(in classType);

		var classMap = _mappings.Scan((classType, SerializerDirection.Deserialize));
		if (classMap is null) throw new ClassMapNotFoundException(in classType);

		if (classType == typeof(string)) return dataObject.ToString();

		var matchingTagName = classMap.NamingStrategy.SafeGetName(in classType, new NamingContext(_mappings));
		if (dataObject.Name != matchingTagName) throw new MissingNodeException(in classType, in matchingTagName);

		var instance = Activator.CreateInstance(classType)!;
		foreach (var propertyMapping in classMap.PropertyMaps)
		{

			var realPropertyInfo = classType.GetProperty(propertyMapping.Property.Name)!;
			var serializerContext = new SerializerContext(
				in realPropertyInfo, in classType, propertyMapping.NamingStrategy, 
				currentSerializer,
				classMap.PropertyMaps, _mappings);

			var propertyName = propertyMapping.NamingStrategy.SafeGetName(in realPropertyInfo, serializerContext);
			if (propertyMapping.Direction == SerializerDirection.Serialize) continue;

			DeserializeProperty(in dataObject, in propertyName, in propertyMapping, in instance, in currentSerializer, in serializerContext);
		}

		return instance;
	}

	private void DeserializeProperty(
		in IXmlElement dataObject, in string propertyName, in IPropertyMap propertyMapping, in object instance, 
		in IXmlSerializer currentSerializer, in SerializerContext serializerContext)
	{
		if (propertyMapping.ContainerType == typeof(IXmlText))
		{
			// This may look strange here but it makes DeserializeNode a lot simpler
			var text = Text(dataObject.GetTextValue());
			DeserializeNode(in text, in dataObject, text.Value, in propertyName, in propertyMapping, in instance, in currentSerializer, in serializerContext);
			return;
		}

		if (propertyMapping.ContainerType == typeof(IXmlAttribute))
		{
			var attribute = dataObject.GetChildAttribute(in propertyName);
			DeserializeNode(in attribute, in dataObject, attribute?.Value, in propertyName, in propertyMapping, in instance, in currentSerializer, in serializerContext);
			return;
		}

		if (propertyMapping.ContainerType == typeof(IXmlElement))
		{
			var element = dataObject.GetChildElement(in propertyName);
			DeserializeElement(in element, in dataObject, in propertyName, in propertyMapping, in instance, in currentSerializer, in serializerContext);
			return;
		}

		throw new ContainerNotSupportedException(propertyMapping.ContainerType);
	}

	private static void DeserializeNode<TNode>(
		in TNode? node, in IXmlElement parent, in string? nodeValue, in string propertyName, in IPropertyMap propertyMapping, in object instance, 
		in IXmlSerializer currentSerializer, in SerializerContext serializerContext)
		where TNode : IXmlNode
	{
		if (nodeValue is null && !propertyMapping.Property.IsNullable())
			throw new ContainerNotFoundException(propertyMapping.Property.PropertyType, propertyMapping.ContainerType, in propertyName);
		if (nodeValue is null)
		{
			SetPropertyValue(in instance, in propertyMapping, null);
			return;
		}

		var converter = propertyMapping.GetConverter<TNode>(SerializerDirection.Deserialize, currentSerializer);
		if (converter is null) throw new ConverterNotFoundException(
			propertyMapping.Property.PropertyType, propertyMapping.ContainerType, SerializerDirection.Deserialize);

		var convertedAttributeValue = converter is IXmlConverter<TNode> xmlConverter
			? xmlConverter.Deserialize(in node!, in parent, serializerContext)
			: converter.Deserialize(in node!, serializerContext);
		if (convertedAttributeValue is null && !propertyMapping.Property.IsNullable())
			throw new NullValueNotAllowedException(propertyMapping.Property.PropertyType, in propertyName);

		SetPropertyValue(in instance, in propertyMapping, in convertedAttributeValue);
	}

	private void DeserializeElement(
		in IXmlElement? element, in IXmlElement parent, in string propertyName, in IPropertyMap propertyMapping, in object instance, 
		in IXmlSerializer currentSerializer, in SerializerContext serializerContext)
	{
		// Collections may be empty
		if (element is null && propertyMapping.Property.PropertyType.IsEnumerable()) return;

		if (element is null && !propertyMapping.Property.IsNullable())
			throw new ContainerNotFoundException(propertyMapping.Property.PropertyType, propertyMapping.ContainerType, in propertyName);
		if (element is null)
		{
			SetPropertyValue(in instance, in propertyMapping, null);
			return;
		}

		var matchingConverter = propertyMapping.GetConverter<IXmlElement>(SerializerDirection.Deserialize, currentSerializer);
		if (matchingConverter is null)
		{
			var deserializedInstance = DeserializeFromElement(in element, propertyMapping.Property.PropertyType, currentSerializer);
			if (deserializedInstance is null && !propertyMapping.Property.IsNullable())
				throw new NullValueNotAllowedException(propertyMapping.Property.PropertyType, in propertyName);

			SetPropertyValue(in instance, in propertyMapping, in deserializedInstance);
			return;
		}

		var convertedInstance = matchingConverter is IXmlConverter<IXmlElement> xmlConverter
			? xmlConverter.Deserialize(in element, in parent, serializerContext)
			: matchingConverter.Deserialize(in element, serializerContext);
		if (convertedInstance is null && !propertyMapping.Property.IsNullable())
			throw new NullValueNotAllowedException(propertyMapping.Property.PropertyType, in propertyName);

		SetPropertyValue(in instance, in propertyMapping, in convertedInstance);
	}

	private static void SetPropertyValue(in object? instance, in IPropertyMap propertyMapping, in object? convertedInstance)
	{
		var propertyInstance = instance!.GetType().GetProperty(propertyMapping.Property.Name);
		propertyInstance!.SetValue(instance, convertedInstance, null);
	}
}