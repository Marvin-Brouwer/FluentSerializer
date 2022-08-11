using Ardalis.GuardClauses;

using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.SerializerException;
using FluentSerializer.Json.Converting.Converters;
using FluentSerializer.Json.DataNodes;

using System;
using System.Collections;
using System.Collections.Generic;

namespace FluentSerializer.Json.Services;

/// <summary>
/// Deserializer for JSON using profiles
/// </summary>
public sealed class JsonTypeDeserializer
{
	private const SerializerDirection TypeDeserializerDirection = SerializerDirection.Deserialize;

	private readonly IClassMapCollection _classMappings;

	/// <inheritdoc cref="JsonTypeDeserializer" />
	public JsonTypeDeserializer(in IClassMapCollection classMapCollection)
	{
		Guard.Against.Null(classMapCollection, nameof(classMapCollection));

		_classMappings = classMapCollection;
	}

	/// <summary>
	/// Deserialize an <see cref="IJsonNode"/> to the requested object type
	/// </summary>
	public TModel? DeserializeFromNode<TModel>(in IJsonNode dataObject, in ISerializerCoreContext<IJsonNode> coreContext)
		where TModel : new()
	{
		Guard.Against.Null(dataObject, nameof(dataObject));
		Guard.Against.Null(coreContext, nameof(coreContext));

		var classType = typeof(TModel);
		var deserializedInstance = DeserializeFromNode(in dataObject, in classType, in coreContext);
		if (deserializedInstance is null) return default;

		return (TModel)deserializedInstance;
	}

	/// <summary>
	/// Deserialize an <see cref="IJsonNode"/> to the requested object type
	/// </summary>
	public object? DeserializeFromNode(in IJsonNode dataObject, in Type classType, in ISerializerCoreContext<IJsonNode> coreContext)
	{
		Guard.Against.Null(dataObject, nameof(dataObject));
		Guard.Against.Null(classType, nameof(classType));
		Guard.Against.Null(coreContext, nameof(coreContext));

		var currentCoreContext = coreContext.WithPathSegment(classType);
		if (typeof(IEnumerable).IsAssignableFrom(classType))
		{
			// The way this works loses the parent context when deserializing collections.
			// Currently, there is no reason to support this and since it'll cause a parent to be passed around
			// through the serializer method chain we figured we skip this feature.
			if (dataObject is not IJsonArray array) throw new ContainerNotSupportedException(in classType);
			return CollectionConverter.DeserializeCollection(in array, in classType,
				currentCoreContext.WithPathSegment(typeof(IEnumerable)));
		}
		if (dataObject is not IJsonObject jsonObject) throw new ContainerNotSupportedException(in classType);

		var classMap = _classMappings.GetClassMapFor(classType, TypeDeserializerDirection);
		if (classMap is null) throw new ClassMapNotFoundException(in classType);

		if (classType == typeof(string)) return dataObject.ToString();

		var instance = Activator.CreateInstance(classType)!;
		foreach (var propertyMapping in classMap.GetAllPropertyMaps(TypeDeserializerDirection))
		{
			var realPropertyInfo = classType.GetProperty(propertyMapping.Property.Name)!;
			var serializerContext = new SerializerContext<IJsonNode>(
				currentCoreContext.WithPathSegment(propertyMapping.Property),
				in realPropertyInfo, realPropertyInfo.PropertyType, in classType, propertyMapping.NamingStrategy, 
				classMap.PropertyMapCollection, _classMappings)
			{
				ParentNode = dataObject
			};
			
			var concretePropertyType = Nullable.GetUnderlyingType(realPropertyInfo.PropertyType) ?? realPropertyInfo.PropertyType;
			var propertyName = propertyMapping.NamingStrategy.SafeGetName(in realPropertyInfo, in concretePropertyType, serializerContext);
			
			DeserializeProperty(in jsonObject, in propertyName, in propertyMapping, in instance, in serializerContext);
		}

		return instance;
	}

	private void DeserializeProperty(
		in IJsonObject dataObject, in string propertyName, in IPropertyMap propertyMapping, in object instance, 
		in SerializerContext<IJsonNode> serializerContext)
	{
		if (propertyMapping.ContainerType == typeof(IJsonProperty))
		{
			var jsonProperty = dataObject.GetProperty(in propertyName);
			var propertyValue = jsonProperty?.Value;
			DeserializeJsonNode(in propertyValue, in propertyName, in propertyMapping, in instance, serializerContext);
			return;
		}
		throw new ContainerNotSupportedException(propertyMapping.ContainerType);
	}

	private void DeserializeJsonNode(
		in IJsonNode? propertyValue, in string propertyName, in IPropertyMap propertyMapping, in object instance, 
		in SerializerContext<IJsonNode> serializerContext)
	{
		var empty = propertyValue is null || propertyValue is IJsonValue jsonValue && string.IsNullOrEmpty(jsonValue.Value);
		// Collections may be empty
		if (empty && propertyMapping.Property.PropertyType.IsEnumerable()) return;

		if (empty && !propertyMapping.Property.IsNullable())
			throw new ContainerNotFoundException(propertyMapping.Property.PropertyType, instance.GetType(), in propertyName);
		if (empty)
		{
			SetPropertyValue(in instance, in propertyMapping, null);
			return;
		}

		var matchingConverter = propertyMapping.GetConverter<IJsonNode, IJsonNode>(TypeDeserializerDirection, serializerContext.CurrentSerializer);
		if (matchingConverter is null)
		{
			var deserializedInstance = DeserializeFromNode(in propertyValue!, propertyMapping.Property.PropertyType, serializerContext);
			if (deserializedInstance is null && !propertyMapping.Property.IsNullable())
				throw new NullValueNotAllowedException(propertyMapping.Property.PropertyType, in propertyName);

			SetPropertyValue(in instance, in propertyMapping, in deserializedInstance);
			return;
		}

		var convertedInstance = matchingConverter.Deserialize(in propertyValue!, serializerContext);
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