using Ardalis.GuardClauses;

using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.SerializerException;
using FluentSerializer.Json.Converting.Converters;
using FluentSerializer.Json.DataNodes;

using System;
using System.Collections;
using System.Collections.Generic;

using static FluentSerializer.Json.JsonBuilder;

namespace FluentSerializer.Json.Services;

/// <summary>
/// Serializer for JSON using profiles
/// </summary>
public sealed class JsonTypeSerializer
{
	private const SerializerDirection TypeSerializerDirection = SerializerDirection.Serialize;

	private readonly IClassMapCollection _classMappings;

	/// <inheritdoc cref="JsonTypeSerializer" />
	public JsonTypeSerializer(in IClassMapCollection classMapCollection)
	{
		Guard.Against.Null(classMapCollection, nameof(classMapCollection));

		_classMappings = classMapCollection;
	}

	/// <summary>
	/// Serialize an instance to an <see cref="IJsonNode"/>
	/// </summary>
	/// <exception cref="NotImplementedException"></exception>
	/// <exception cref="ClassMapNotFoundException"></exception>
	public IJsonNode? SerializeToNode(in object dataModel, in Type classType, in ISerializerCoreContext coreContext)
	{
		Guard.Against.Null(dataModel, nameof(dataModel));
		Guard.Against.Null(classType, nameof(classType));
		Guard.Against.Null(coreContext, nameof(coreContext));

		var currentCoreContext = coreContext.WithPathSegment(classType);
		if (typeof(IEnumerable).IsAssignableFrom(classType))
		{
			if (dataModel is not IEnumerable enumerable) throw new ContainerNotSupportedException(in classType);
			return CollectionConverter.SerializeCollection(in enumerable, currentCoreContext.WithPathSegment(typeof(IEnumerable)));
		}

		var instanceType = dataModel.GetType();
		var classMap =
			_classMappings.GetClassMapFor(instanceType, TypeSerializerDirection) ??
			_classMappings.GetClassMapFor(classType, TypeSerializerDirection);
		if (classMap is null) throw new ClassMapNotFoundException(in classType);

		currentCoreContext.TryAddReference(dataModel);

		var properties = new List<IJsonObjectContent>();
		foreach(var property in instanceType.GetProperties())
		{
			var propertyMapping = classMap.GetPropertyMapFor(property, SerializerDirection.Deserialize);
			if (propertyMapping is null) continue;

			var propertyValue = property.GetValue(dataModel);
			if (propertyValue is null && !coreContext.CurrentSerializer.Configuration.WriteNull) continue;

			if (currentCoreContext.ContainsReference(propertyValue))
			{
				var referenceLoopBehavior = currentCoreContext.CurrentSerializer.Configuration.ReferenceLoopBehavior;
				if (referenceLoopBehavior == ReferenceLoopBehavior.Ignore) continue;

				throw new ReferenceLoopException(in instanceType, property.PropertyType, property.Name);
			}

			var serializerContext = new SerializerContext(
				currentCoreContext.WithPathSegment(propertyMapping.Property),
				propertyMapping.Property, property.PropertyType, in classType, propertyMapping.NamingStrategy, 
				classMap.PropertyMapCollection, _classMappings);

			var jsonNode = SerializeObjectContent(in propertyValue, in propertyMapping, in serializerContext);
			if (jsonNode is not null) properties.Add(jsonNode);
		}

		return Object(properties);
	}

	private IJsonObjectContent? SerializeObjectContent(
		in object? propertyValue, in IPropertyMap propertyMapping, in SerializerContext serializerContext)
	{
		if (typeof(IJsonProperty).IsAssignableFrom(propertyMapping.ContainerType))
		{
			var propertyName = propertyMapping.NamingStrategy
				.GetName(propertyMapping.Property, serializerContext.PropertyType, serializerContext)
				.ToString();

			if (propertyValue is null && !serializerContext.CurrentSerializer.Configuration.WriteNull) return null;
			if (propertyValue is null && serializerContext.CurrentSerializer.Configuration.WriteNull) return Property(in propertyName, Value(null));

			var jsonPropertyValue = SerializeProperty(in propertyValue!, in propertyMapping, in serializerContext, propertyName);
			if (jsonPropertyValue is null && !serializerContext.CurrentSerializer.Configuration.WriteNull) return null;
			if (jsonPropertyValue is null && serializerContext.CurrentSerializer.Configuration.WriteNull) return Property(in propertyName, Value(null));

			return jsonPropertyValue;
		}

		throw new ContainerNotSupportedException(propertyMapping.ContainerType);
	}

	private IJsonObjectContent? SerializeProperty(in object propertyValue, in IPropertyMap propertyMapping,
		in SerializerContext serializerContext, string propertyName)
	{
		var matchingConverter = propertyMapping.GetConverter<IJsonNode, IJsonNode>(
			TypeSerializerDirection, serializerContext.CurrentSerializer);

		var jsonPropertyValue = matchingConverter is not null
			? matchingConverter.Serialize(in propertyValue, serializerContext)
			: SerializeToNode(in propertyValue, serializerContext.PropertyType, serializerContext);

		if (jsonPropertyValue is not IJsonPropertyContent jsonPropertyContent) return null;
		return Property(in propertyName, in jsonPropertyContent);
	}
}