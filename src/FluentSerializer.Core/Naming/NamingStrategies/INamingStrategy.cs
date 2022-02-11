using System;
using System.Reflection;
using FluentSerializer.Core.Context;

namespace FluentSerializer.Core.Naming.NamingStrategies;

/// <summary>
/// A strategy to determine how to transform a class or property to a valid serializable name
/// </summary>
public interface INamingStrategy
{
	/// <summary>
	/// Return a valid serializable name value for this <paramref name="property"/>
	/// </summary>
	public string GetName(in PropertyInfo property, in Type propertyType, in INamingContext namingContext);
	/// <summary>
	/// Return a valid serializable name value for this <paramref name="classType"/>
	/// </summary>
	public string GetName(in Type classType, in INamingContext namingContext);
}