using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Naming.NamingStrategies;

using System;
using System.Collections.Generic;
using System.Reflection;

namespace FluentSerializer.Core.Mapping;

/// <inheritdoc />
public sealed class NewClassMap : INewClassMap
{
	private readonly Func<INamingStrategy> _namingStrategy;
	private readonly PropertyMapCollection _propertyMaps;

	/// <inheritdoc />
	public INamingStrategy NamingStrategy => _namingStrategy();

	/// <inheritdoc />
	public Type ClassType { get; }


	/// <inheritdoc />
	public SerializerDirection Direction { get; }

	/// <inheritdoc cref="ClassMap" />
	public NewClassMap(
		in Type classType,
		in SerializerDirection direction,
		in Func<INamingStrategy> namingStrategy,
		in IReadOnlyList<IPropertyMap> propertyMap)
	{
		_namingStrategy = namingStrategy;
		_propertyMaps = new PropertyMapCollection(propertyMap);

		Direction = direction;
		ClassType = Nullable.GetUnderlyingType(classType) ?? classType;
	}

	/// <inheritdoc />
	public IReadOnlyList<IPropertyMap> GetAllPropertyMaps() =>
		_propertyMaps.GetAllPropertyMaps();

	/// <inheritdoc />
	public IPropertyMap? GetPropertyMapFor(in PropertyInfo propertyInfo, in SerializerDirection direction) =>
		_propertyMaps.GetPropertyMapFor(in propertyInfo, in direction);
}