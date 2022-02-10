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
using FluentSerializer.Json.Profiles;
using FluentSerializer.Json.Converting.Converters;

namespace FluentSerializer.Json.Services;

/// <summary>
/// Serializer for JSON using profiles
/// </summary>
public sealed class JsonTypeSerializer
{
	private readonly IClassMapScanList<JsonSerializerProfile> _mappings;

	/// <inheritdoc cref="JsonTypeSerializer" />
	public JsonTypeSerializer(in IClassMapScanList<JsonSerializerProfile> mappings)
	{
		Guard.Against.Null(mappings, nameof(mappings));

		_mappings = mappings;
	}

	/// <summary>
	/// Serialize an instance to an <see cref="IJsonNode"/>
	/// </summary>
	/// <exception cref="NotImplementedException"></exception>
	/// <exception cref="ClassMapNotFoundException"></exception>
	public IJsonNode? SerializeToNode(in object dataModel, in Type classType, in IJsonSerializer currentSerializer)
	{
		Guard.Against.Null(dataModel, nameof(dataModel));
		Guard.Against.Null(classType, nameof(classType));
		Guard.Against.Null(currentSerializer, nameof(currentSerializer));

		if (typeof(IEnumerable).IsAssignableFrom(classType))
		{
			if (dataModel is not IEnumerable enumerable) throw new ContainerNotSupportedException(in classType);
			return CollectionConverter.SerializeCollection(in enumerable, currentSerializer);
		}

		var classMap = _mappings.Scan((classType, SerializerDirection.Serialize));
		if (classMap is null) throw new ClassMapNotFoundException(in classType);

		var properties = new List<IJsonObjectContent>();
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

			var jsonNode = SerializeObjectContent(in propertyValue, in propertyMapping, in currentSerializer, in serializerContext);
			if (jsonNode is not null) properties.Add(jsonNode);
		}

		return Object(properties);
	}

	private IJsonObjectContent? SerializeObjectContent(
		in object propertyValue, in IPropertyMap propertyMapping,  
		in IJsonSerializer currentSerializer, in SerializerContext serializerContext)
	{
		if (typeof(IJsonProperty).IsAssignableFrom(propertyMapping.ContainerType))
		{
			return SerializeProperty(in propertyValue, in propertyMapping, in serializerContext, in currentSerializer);
		}

		throw new ContainerNotSupportedException(propertyMapping.ContainerType);
	}

	private IJsonObjectContent? SerializeProperty(in object propertyValue, in IPropertyMap propertyMapping,
		in SerializerContext serializerContext, in IJsonSerializer currentSerializer)
	{
		var matchingConverter = propertyMapping.GetConverter<IJsonNode>(
			SerializerDirection.Serialize, serializerContext.CurrentSerializer);

		var nodeValue = matchingConverter is null 
			? SerializeToNode(in propertyValue, serializerContext.PropertyType, currentSerializer) 
			: matchingConverter.Serialize(in propertyValue, serializerContext);
		if (nodeValue is not IJsonPropertyContent jsonContent) return default;

		var propertyName = propertyMapping.NamingStrategy.GetName(propertyMapping.Property, serializerContext);
            
		return Property(in propertyName, in jsonContent);
	}
}