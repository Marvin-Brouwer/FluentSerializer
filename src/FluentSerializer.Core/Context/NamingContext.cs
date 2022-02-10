using Ardalis.GuardClauses;
using FluentSerializer.Core.Mapping;
using System;
using System.Reflection;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Naming.NamingStrategies;

namespace FluentSerializer.Core.Context;

/// <inheritdoc cref="INamingContext" />
public class NamingContext : INamingContext
{
	private readonly IScanList<(Type type, SerializerDirection direction), IClassMap> _classMappings;

	/// <inheritdoc cref="NamingContext" />
	public NamingContext(in IScanList<(Type type, SerializerDirection direction), IClassMap> classMappings)
	{
		_classMappings = classMappings;
	}

	/// <inheritdoc />
	public INamingStrategy? FindNamingStrategy(in Type classType, in PropertyInfo property)
	{
		Guard.Against.Null(classType, nameof(classType));
		Guard.Against.Null(property, nameof(property));

		var classMap = _classMappings.Scan((classType, SerializerDirection.Both));
		if (classMap is null) return null;

		return FindNamingStrategy(classMap.PropertyMaps, in property);
	}

	/// <inheritdoc cref="INamingContext" />
	protected static INamingStrategy? FindNamingStrategy(in IScanList<PropertyInfo, IPropertyMap> propertyMapping, in PropertyInfo property)
	{
		Guard.Against.Null(propertyMapping, nameof(propertyMapping));
		Guard.Against.Null(property, nameof(property));

		return propertyMapping.Scan(property)?.NamingStrategy;
	}

	/// <inheritdoc />
	public INamingStrategy? FindNamingStrategy(in Type type)
	{
		Guard.Against.Null(type, nameof(type));

		return _classMappings.Scan((type, SerializerDirection.Both))?.NamingStrategy;
	}
}