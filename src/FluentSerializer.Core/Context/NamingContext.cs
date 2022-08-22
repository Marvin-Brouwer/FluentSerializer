using Ardalis.GuardClauses;

using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.Naming.NamingStrategies;

using System;
using System.Reflection;

namespace FluentSerializer.Core.Context;

/// <inheritdoc cref="INamingContext" />
public class NamingContext : INamingContext
{
	private readonly IClassMapCollection _classMappings;

	/// <inheritdoc cref="NamingContext" />
	public NamingContext(in IClassMapCollection classMappings)
	{
		_classMappings = classMappings;
	}

	/// <inheritdoc />
	public INamingStrategy? FindNamingStrategy(in Type classType, in PropertyInfo propertyInfo)
	{
		Guard.Against.Null(classType, nameof(classType));
		Guard.Against.Null(propertyInfo, nameof(propertyInfo));

		var classMap = _classMappings.GetClassMapFor(in classType);
		if (classMap is null) return null;

		return classMap.GetPropertyMapFor(in propertyInfo)?.NamingStrategy;
	}

	/// <inheritdoc cref="INamingContext" />
	public static INamingStrategy? FindNamingStrategy(in IPropertyMapCollection propertyMapping, in PropertyInfo property)
	{
		Guard.Against.Null(propertyMapping, nameof(propertyMapping));
		Guard.Against.Null(property, nameof(property));

		return propertyMapping.GetPropertyMapFor(in property)?.NamingStrategy;
	}

	/// <inheritdoc />
	public INamingStrategy? FindNamingStrategy(in Type type)
	{
		Guard.Against.Null(type, nameof(type));

		return _classMappings.GetClassMapFor(type)?.NamingStrategy;
	}
}