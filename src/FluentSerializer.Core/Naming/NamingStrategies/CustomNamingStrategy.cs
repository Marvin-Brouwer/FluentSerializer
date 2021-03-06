using System;
using System.Reflection;
using FluentSerializer.Core.Context;

namespace FluentSerializer.Core.Naming.NamingStrategies;

/// <summary>
/// Use a preconfigured value
/// </summary>
public sealed class CustomNamingStrategy : INamingStrategy
{
	private readonly string _name;

	/// <summary>
	/// Use <paramref name="name"/> as a preconfigured value for this strategy
	/// </summary>
	public CustomNamingStrategy(in string name)
	{
		_name = name;
	}

	/// <inheritdoc />
	public string GetName(in PropertyInfo property, in Type propertyType, in INamingContext namingContext) => _name;
	/// <inheritdoc />
	public string GetName(in Type classType, in INamingContext namingContext) => _name;
}