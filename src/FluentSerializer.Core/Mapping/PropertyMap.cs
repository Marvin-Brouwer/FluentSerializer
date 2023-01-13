using Ardalis.GuardClauses;

using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Converting;
using FluentSerializer.Core.DataNodes;
using FluentSerializer.Core.Naming.NamingStrategies;
using FluentSerializer.Core.SerializerException;
using FluentSerializer.Core.Services;

using System;
using System.Linq;
using System.Reflection;

namespace FluentSerializer.Core.Mapping;

/// <inheritdoc />
public sealed class PropertyMap : IPropertyMap
{

	private readonly Func<INamingStrategy> _namingStrategy;
	private readonly Func<IConverter>? _customConverter;

	/// <inheritdoc />
	public INamingStrategy NamingStrategy => _namingStrategy();
	/// <inheritdoc />
	public IConverter? CustomConverter => _customConverter?.Invoke();

	/// <inheritdoc />
	public SerializerDirection Direction { get; }
	/// <inheritdoc />
	public PropertyInfo Property { get; }
	/// <inheritdoc />
	public Type ConcretePropertyType { get; }
	/// <inheritdoc />
	public Type ContainerType { get; }

	/// <inheritdoc cref="PropertyMap" />
	public PropertyMap(
		in SerializerDirection direction,
		in Type containerType,
		in PropertyInfo property,
		in Func<INamingStrategy> namingStrategy,
		in Func<IConverter>? customConverter)
	{
		Guard.Against.Null(containerType
#if NETSTANDARD
			, nameof(containerType)
#endif
		);
		Guard.Against.Null(property
#if NETSTANDARD
			, nameof(property)
#endif
		);
		Guard.Against.Null(namingStrategy
#if NETSTANDARD
			, nameof(namingStrategy)
#endif
		);

		_namingStrategy = namingStrategy;
		_customConverter = customConverter;

		Direction = direction;
		Property = property;
		ConcretePropertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
		ContainerType = containerType;
	}

	/// <inheritdoc />
	public IConverter<TDataContainer, TDataNode>? GetConverter<TDataContainer, TDataNode>(
		SerializerDirection direction, in ISerializer currentSerializer)
		where TDataContainer : IDataNode
		where TDataNode : IDataNode
	{
		Guard.Against.Null(currentSerializer
#if NETSTANDARD
			, nameof(currentSerializer)
#endif
		);

		var converter = CustomConverter ?? currentSerializer.Configuration.DefaultConverters
			.Where(converter => converter is IConverter<TDataContainer, TDataNode>)
			.Where(converter => converter.Direction == SerializerDirection.Both || converter.Direction == direction)
			.FirstOrDefault(converter => converter.CanConvert(ConcretePropertyType));
		if (converter is null) return null;

		if (!converter.CanConvert(ConcretePropertyType))
			throw new ConverterNotSupportedException(Property, ConcretePropertyType, converter.GetType(), typeof(TDataContainer), direction);
		if (converter is IConverter<TDataContainer, TDataNode> specificConverter)
			return specificConverter;

		throw new ConverterNotSupportedException(Property, ConcretePropertyType, converter.GetType(), typeof(TDataContainer), direction);
	}
}