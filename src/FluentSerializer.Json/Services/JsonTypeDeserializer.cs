using Ardalis.GuardClauses;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.SerializerException;
using System;
using System.Collections;
using FluentSerializer.Json.DataNodes;
using FluentSerializer.Json.Profiles;

namespace FluentSerializer.Json.Services;

/// <summary>
/// Deserializer for JSON using profiles
/// </summary>
public sealed class JsonTypeDeserializer
{
	private readonly IClassMapScanList<JsonSerializerProfile> _mappings;

	/// <inheritdoc />
	public JsonTypeDeserializer(in IClassMapScanList<JsonSerializerProfile> mappings)
	{
		Guard.Against.Null(mappings, nameof(mappings));

		_mappings = mappings;
	}

	/// <summary>
	/// Deserialize an <see cref="IJsonNode"/> to the requested object type
	/// </summary>
	public TModel? DeserializeFromNode<TModel>(in IJsonNode dataObject, in IJsonSerializer currentSerializer)
		where TModel : new()
	{
		Guard.Against.Null(dataObject, nameof(dataObject));
		Guard.Against.Null(currentSerializer, nameof(currentSerializer));

		var classType = typeof(TModel);
		var deserializedInstance = DeserializeFromNode(in dataObject, in classType, in currentSerializer);
		if (deserializedInstance is null) return default;

		return (TModel)deserializedInstance;
	}

	/// <summary>
	/// Deserialize an <see cref="IJsonNode"/> to the requested object type
	/// </summary>
	public object? DeserializeFromNode(in IJsonNode dataObject, in Type classType,  in IJsonSerializer currentSerializer)
	{
		Guard.Against.Null(dataObject, nameof(dataObject));
		Guard.Against.Null(classType, nameof(classType));
		Guard.Against.Null(currentSerializer, nameof(currentSerializer));

		if (typeof(IEnumerable).IsAssignableFrom(classType)) throw new NotImplementedException("Todo");
		if (dataObject is not IJsonObject jsonObject)  throw new NotImplementedException("Todo");

		var classMap = _mappings.Scan((classType, SerializerDirection.Deserialize));
		if (classMap is null) throw new ClassMapNotFoundException(in classType);

		if (classType == typeof(string)) return dataObject.ToString();
			
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

			DeserializeProperty(in jsonObject, in propertyName, in propertyMapping, in instance, in currentSerializer, in serializerContext);
		}

		return instance;
	}

	private void DeserializeProperty(
		in IJsonObject dataObject, in string propertyName, in IPropertyMap propertyMapping, in object instance, 
		in IJsonSerializer currentSerializer, in SerializerContext serializerContext)
	{
		if (propertyMapping.ContainerType == typeof(IJsonProperty))
		{
			var jsonProperty = dataObject.GetProperty(in propertyName);
			var propertyValue = jsonProperty?.Value;
			DeserializeXElement(in propertyValue, in propertyName, in propertyMapping, in instance, in currentSerializer, in serializerContext);
			return;
		}

		throw new ContainerNotSupportedException(propertyMapping.ContainerType);
	}

	private void DeserializeXElement(
		in IJsonNode? propertyValue, in string propertyName, in IPropertyMap propertyMapping, in object instance, 
		in IJsonSerializer currentSerializer, in SerializerContext serializerContext)
	{
		var empty = propertyValue is null || propertyValue is IJsonValue jsonValue && string.IsNullOrEmpty(jsonValue.Value);
		// Collections may be empty
		if (empty && propertyMapping.Property.PropertyType.IsEnumerable()) return;

		if (empty && !propertyMapping.Property.IsNullable())
			throw new ContainerNotFoundException(propertyMapping.Property.PropertyType, propertyMapping.ContainerType, in propertyName);
		if (empty)
		{
			SetPropertyValue(in instance, in propertyMapping, null);
			return;
		}

		var matchingConverter = propertyMapping.GetConverter<IJsonNode>(SerializerDirection.Deserialize, currentSerializer);
		if (matchingConverter is null)
		{
			var deserializedInstance = DeserializeFromNode(in propertyValue!, propertyMapping.Property.PropertyType, in currentSerializer);
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