using FluentSerializer.Core.Context;

using System;
using System.Reflection;

namespace FluentSerializer.Core.Naming.NamingStrategies;

/// <summary>
/// Use a preconfigured value
/// </summary>
public readonly struct CustomNamingStrategy : INamingStrategy
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
	public ReadOnlySpan<char> GetName(in PropertyInfo propertyInfo, in Type propertyType, in INamingContext namingContext) => _name;
	/// <inheritdoc />
	public ReadOnlySpan<char> GetName(in Type classType, in INamingContext namingContext) => _name;
}