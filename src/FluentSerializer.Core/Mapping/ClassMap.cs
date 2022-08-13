using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Naming.NamingStrategies;

using System;
using System.Collections.Generic;
using System.Reflection;

namespace FluentSerializer.Core.Mapping;

/// <inheritdoc />
public sealed class ClassMap : IClassMap
{
	private readonly Func<INamingStrategy> _namingStrategy;

	/// <inheritdoc />
	public INamingStrategy NamingStrategy => _namingStrategy();

	/// <inheritdoc />
	public Type ClassType { get; }

	/// <inheritdoc />
	public SerializerDirection Direction { get; }

	/// <inheritdoc />
	public IPropertyMapCollection PropertyMapCollection { get; private set; }

	/// <inheritdoc cref="ClassMap" />
	public ClassMap(
		in Type classType,
		in SerializerDirection direction,
		in Func<INamingStrategy> namingStrategy,
		in IReadOnlyCollection<IPropertyMap> propertyMap)
	{
		_namingStrategy = namingStrategy;

		Direction = direction;
		ClassType = Nullable.GetUnderlyingType(classType) ?? classType;
		PropertyMapCollection = new PropertyMapCollection(in propertyMap);
	}

	/// <inheritdoc cref="IPropertyMapCollection" />
	public IReadOnlyCollection<IPropertyMap> GetAllPropertyMaps(in SerializerDirection direction) =>
		PropertyMapCollection.GetAllPropertyMaps(in direction);

	/// <inheritdoc cref="IPropertyMapCollection" />
	public IPropertyMap? GetPropertyMapFor(in PropertyInfo propertyInfo, in SerializerDirection direction) =>
		PropertyMapCollection.GetPropertyMapFor(in propertyInfo, in direction);

	/// <inheritdoc cref="IPropertyMapCollection" />
	public IPropertyMap? GetPropertyMapFor(in PropertyInfo propertyInfo) =>
		PropertyMapCollection.GetPropertyMapFor(in propertyInfo);
}