using Ardalis.GuardClauses;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.SerializerException;
using System;
using System.Collections;
using FluentSerializer.Json.DataNodes;
using System.Collections.Generic;

using static FluentSerializer.Json.JsonBuilder;

namespace FluentSerializer.Json.Services;

/// <summary>
/// Serializer for JSON using profiles
/// </summary>
public sealed class JsonTypeSerializer
{
	private readonly IScanList<(Type type, SerializerDirection direction), IClassMap> _mappings;

	/// <inheritdoc />
	public JsonTypeSerializer(IScanList<(Type type, SerializerDirection direction), IClassMap> mappings)
	{
		Guard.Against.Null(mappings, nameof(mappings));

		_mappings = mappings;
	}

	/// <summary>
	/// Serialize an instance to an <see cref="IJsonNode"/>
	/// </summary>
	/// <exception cref="NotImplementedException"></exception>
	/// <exception cref="ClassMapNotFoundException"></exception>
	public IJsonNode? SerializeToNode(object dataModel, Type classType, IJsonSerializer currentSerializer)
	{
		Guard.Against.Null(dataModel, nameof(dataModel));
		Guard.Against.Null(classType, nameof(classType));
		Guard.Against.Null(currentSerializer, nameof(currentSerializer));

		if (typeof(IEnumerable).IsAssignableFrom(classType)) throw new NotImplementedException("Todo");

		var classMap = _mappings.Scan((classType, SerializerDirection.Serialize));
		if (classMap is null) throw new ClassMapNotFoundException(classType);

		var properties = new List<IJsonObjectContent>();
		foreach(var property in classType.GetProperties())
		{
			var propertyMapping = classMap.PropertyMaps.Scan(property);
			if (propertyMapping is null) continue;
			if (propertyMapping.Direction == SerializerDirection.Deserialize) continue;

			var propertyValue = property.GetValue(dataModel);
			if (propertyValue is null) continue;

			var serializerContext = new SerializerContext(
				propertyMapping.Property, classType, propertyMapping.NamingStrategy, 
				currentSerializer,
				classMap.PropertyMaps, _mappings);

			var jsonNode = SerializeObjectContent(propertyValue, propertyMapping, currentSerializer, serializerContext);
			if (jsonNode is not null) properties.Add(jsonNode);
		}

		return Object(properties);
	}

	private IJsonObjectContent? SerializeObjectContent(
		object propertyValue, IPropertyMap propertyMapping,  
		IJsonSerializer currentSerializer, SerializerContext serializerContext)
	{
		if (typeof(IJsonProperty).IsAssignableFrom(propertyMapping.ContainerType))
		{
			return SerializeProperty(propertyValue, propertyMapping, serializerContext, currentSerializer);
		}

		throw new ContainerNotSupportedException(propertyMapping.ContainerType);
	}

	private IJsonObjectContent? SerializeProperty(object propertyValue, IPropertyMap propertyMapping,
		SerializerContext serializerContext, IJsonSerializer currentSerializer)
	{
		var matchingConverter = propertyMapping.GetConverter<IJsonNode>(
			SerializerDirection.Serialize, serializerContext.CurrentSerializer);

		var nodeValue = matchingConverter is null 
			? SerializeToNode(propertyValue, serializerContext.PropertyType, currentSerializer) 
			: matchingConverter.Serialize(propertyValue, serializerContext);
		if (nodeValue is not IJsonPropertyContent jsonContent) return default;

		var propertyName = propertyMapping.NamingStrategy.GetName(propertyMapping.Property, serializerContext);
            
		return Property(propertyName, jsonContent);
	}
}