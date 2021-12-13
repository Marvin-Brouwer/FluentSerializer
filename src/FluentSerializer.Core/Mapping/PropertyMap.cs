using Ardalis.GuardClauses;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.SerializerException;
using FluentSerializer.Core.Services;
using System;
using System.Linq;
using System.Reflection;
using FluentSerializer.Core.Converting;
using FluentSerializer.Core.Naming.NamingStrategies;
using FluentSerializer.Core.DataNodes;

namespace FluentSerializer.Core.Mapping;

public sealed class PropertyMap : IPropertyMap 
{

	private readonly Func<INamingStrategy> _namingStrategy;
	private readonly Func<IConverter>? _customConverter;

	public INamingStrategy NamingStrategy => _namingStrategy();
	public IConverter? CustomConverter => _customConverter?.Invoke();

	public SerializerDirection Direction { get; }
	public PropertyInfo Property { get; }
	public Type ConcretePropertyType { get; }
	public Type ContainerType { get; }

	public PropertyMap(
		SerializerDirection direction,
		Type containerType,
		PropertyInfo property,
		Func<INamingStrategy> namingStrategy,
		Func<IConverter>? customConverter)
	{
		_namingStrategy = namingStrategy;
		_customConverter = customConverter;

		Direction = direction;
		Property = property;
		ConcretePropertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
		ContainerType = containerType;
	}

	public IConverter<TDataContainer>? GetConverter<TDataContainer>(
		SerializerDirection direction, ISerializer currentSerializer)
		where TDataContainer : IDataNode
	{
		Guard.Against.Null(direction, nameof(direction));
		Guard.Against.Null(currentSerializer, nameof(currentSerializer));

		var converter = CustomConverter ?? currentSerializer.Configuration.DefaultConverters
			.Where(converter => converter is IConverter<TDataContainer>)
			.Where(converter => converter.Direction == SerializerDirection.Both || converter.Direction == direction)
			.FirstOrDefault(converter => converter.CanConvert(ConcretePropertyType));
		if (converter is null) return null;

		if (!converter.CanConvert(ConcretePropertyType))
			throw new ConverterNotSupportedException(this, converter.GetType(), typeof(TDataContainer), direction);
		if (converter is IConverter<TDataContainer> specificConverter)
			return specificConverter;

		throw new ConverterNotSupportedException(this, converter.GetType(), typeof(TDataContainer), direction);
	}
}