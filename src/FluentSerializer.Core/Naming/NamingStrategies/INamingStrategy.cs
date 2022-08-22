using FluentSerializer.Core.Context;

using System;
using System.Reflection;

namespace FluentSerializer.Core.Naming.NamingStrategies;

/// <summary>
/// A strategy to determine how to transform a class or property to a valid serializable name
/// </summary>
public interface INamingStrategy
{
	/// <summary>
	/// Return a valid serializable name value for this <paramref name="propertyInfo"/>
	/// </summary>
	public ReadOnlySpan<char> GetName(in PropertyInfo propertyInfo, in Type propertyType, in INamingContext namingContext);
	/// <summary>
	/// Return a valid serializable name value for this <paramref name="classType"/>
	/// </summary>
	public ReadOnlySpan<char> GetName(in Type classType, in INamingContext namingContext);
}