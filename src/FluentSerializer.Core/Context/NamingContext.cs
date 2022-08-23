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
		Guard.Against.Null(classType
#if NETSTANDARD2_1
			, nameof(classType)
#endif
		);
		Guard.Against.Null(propertyInfo
#if NETSTANDARD2_1
			, nameof(propertyInfo)
#endif
		);

		var classMap = _classMappings.GetClassMapFor(in classType);
		if (classMap is null) return null;

		return classMap.GetPropertyMapFor(in propertyInfo)?.NamingStrategy;
	}

	/// <inheritdoc cref="INamingContext" />
	public static INamingStrategy? FindNamingStrategy(in IPropertyMapCollection propertyMapping, in PropertyInfo propertyInfo)
	{
		Guard.Against.Null(propertyMapping
#if NETSTANDARD2_1
			, nameof(propertyMapping)
#endif
		);
		Guard.Against.Null(propertyInfo
#if NETSTANDARD2_1
			, nameof(propertyInfo)
#endif
		);

		return propertyMapping.GetPropertyMapFor(in propertyInfo)?.NamingStrategy;
	}

	/// <inheritdoc />
	public INamingStrategy? FindNamingStrategy(in Type type)
	{
		Guard.Against.Null(type
#if NETSTANDARD2_1
			, nameof(type)
#endif
		);

		return _classMappings.GetClassMapFor(type)?.NamingStrategy;
	}
}