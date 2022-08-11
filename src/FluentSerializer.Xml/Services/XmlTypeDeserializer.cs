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
/// Deserializer for XML using profiles
/// </summary>
public sealed class XmlTypeDeserializer
{
	private const SerializerDirection TypeDeserializerDirection = SerializerDirection.Deserialize;

	private readonly IClassMapCollection _classMappings;

	/// <inheritdoc cref="XmlTypeDeserializer" />
	public XmlTypeDeserializer(in IClassMapCollection classMapCollection)
	{
		Guard.Against.Null(classMapCollection, nameof(classMapCollection));

		_classMappings = classMapCollection;
	}

	/// <summary>
	/// Deserialize an <see cref="IXmlElement"/> to the requested object type
	/// </summary>
	public TModel? DeserializeFromElement<TModel>(in IXmlElement dataObject, in ISerializerCoreContext<IXmlNode> context)
		where TModel : new()
	{
		Guard.Against.Null(dataObject, nameof(dataObject));
		Guard.Against.Null(context, nameof(context));

		var classType = typeof(TModel);
		var deserializedInstance = DeserializeFromElement(in dataObject, in classType, in context);
		if (deserializedInstance is null) return default;

		return (TModel)deserializedInstance;
	}

	/// <summary>
	/// Deserialize an <see cref="IXmlElement"/> to the requested object type
	/// </summary>
	public object? DeserializeFromElement(in IXmlElement dataObject, in Type classType, in ISerializerCoreContext<IXmlNode> coreContext)
	{
		Guard.Against.Null(dataObject, nameof(dataObject));
		Guard.Against.Null(classType, nameof(classType));
		Guard.Against.Null(coreContext, nameof(coreContext));

		if (typeof(IEnumerable).IsAssignableFrom(classType)) throw new MalConfiguredRootNodeException(in classType);

		var classMap = _classMappings.GetClassMapFor(classType, TypeDeserializerDirection);
		if (classMap is null) throw new ClassMapNotFoundException(in classType);

		if (classType == typeof(string)) return dataObject.ToString();

		var currentCoreContext = coreContext.WithPathSegment(classType);
		var matchingTagName = classMap.NamingStrategy.SafeGetName(in classType, new NamingContext(_classMappings));
		if (dataObject.Name != matchingTagName) throw new MissingNodeException(in classType, in matchingTagName);

		var instance = Activator.CreateInstance(classType)!;
		foreach (var propertyMapping in classMap.GetAllPropertyMaps(SerializerDirection.Serialize))
		{
			var realPropertyInfo = classType.GetProperty(propertyMapping.Property.Name)!;
			var serializerContext = new SerializerContext<IXmlNode>(
				currentCoreContext.WithPathSegment(propertyMapping.Property),
				in realPropertyInfo, realPropertyInfo.PropertyType, in classType, propertyMapping.NamingStrategy,
				classMap.PropertyMapCollection, in _classMappings)
			{
				ParentNode = dataObject
			};

			var propertyName = propertyMapping.NamingStrategy.SafeGetName(in realPropertyInfo, serializerContext.PropertyType, serializerContext);
			DeserializeProperty(in dataObject, in propertyName, in propertyMapping, in instance, in serializerContext);
		}

		return instance;
	}

	private void DeserializeProperty(
		in IXmlElement dataObject, in string propertyName, in IPropertyMap propertyMapping, in object instance, in SerializerContext<IXmlNode> serializerContext)
	{
		if (propertyMapping.ContainerType == typeof(IXmlText))
		{
			// This may look strange here but it makes DeserializeNode a lot simpler
			var text = Text(dataObject.GetTextValue());
			DeserializeNode(in text, text.Value, in propertyName, in propertyMapping, in instance, in serializerContext);
			return;
		}

		if (propertyMapping.ContainerType == typeof(IXmlAttribute))
		{
			var attribute = dataObject.GetChildAttribute(in propertyName);
			DeserializeNode(in attribute, attribute?.Value, in propertyName, in propertyMapping, in instance, in serializerContext);
			return;
		}

		if (propertyMapping.ContainerType == typeof(IXmlElement))
		{
			var element = dataObject.GetChildElement(in propertyName);
			DeserializeElement(in element, in propertyName, in propertyMapping, in instance, in serializerContext);
			return;
		}

		throw new ContainerNotSupportedException(propertyMapping.ContainerType);
	}

	private static void DeserializeNode<TNode>(
		in TNode? node, in string? nodeValue, in string propertyName, in IPropertyMap propertyMapping, in object instance, in SerializerContext<IXmlNode> serializerContext)
		where TNode : IXmlNode
	{
		if (nodeValue is null && !propertyMapping.Property.IsNullable())
			throw new ContainerNotFoundException(propertyMapping.Property.PropertyType, propertyMapping.ContainerType, in propertyName);
		if (nodeValue is null)
		{
			SetPropertyValue(in instance, in propertyMapping, null);
			return;
		}

		var converter = propertyMapping.GetConverter<TNode, IXmlNode>(TypeDeserializerDirection, serializerContext.CurrentSerializer);
		if (converter is null) throw new ConverterNotFoundException(
			propertyMapping.Property.PropertyType, propertyMapping.ContainerType, TypeDeserializerDirection);

		var convertedAttributeValue = converter.Deserialize(in node!, serializerContext);
		if (convertedAttributeValue is null && !propertyMapping.Property.IsNullable())
			throw new NullValueNotAllowedException(propertyMapping.Property.PropertyType, in propertyName);

		SetPropertyValue(in instance, in propertyMapping, in convertedAttributeValue);
	}

	private void DeserializeElement(
		in IXmlElement? element, in string propertyName, in IPropertyMap propertyMapping, in object instance, in SerializerContext<IXmlNode> serializerContext)
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

		var matchingConverter = propertyMapping.GetConverter<IXmlElement, IXmlNode>(TypeDeserializerDirection, serializerContext.CurrentSerializer);
		if (matchingConverter is null)
		{
			var deserializedInstance = DeserializeFromElement(in element, propertyMapping.Property.PropertyType, serializerContext);
			if (deserializedInstance is null && !propertyMapping.Property.IsNullable())
				throw new NullValueNotAllowedException(propertyMapping.Property.PropertyType, in propertyName);

			SetPropertyValue(in instance, in propertyMapping, in deserializedInstance);
			return;
		}

		var convertedInstance = matchingConverter.Deserialize(in element, serializerContext);
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