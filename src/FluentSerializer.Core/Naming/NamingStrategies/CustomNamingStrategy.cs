using System;
using System.Reflection;
using Ardalis.GuardClauses;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Extensions;

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
		Guard.Against.InvalidName(name);

		_name = name;
	}

	/// <inheritdoc />
	public string GetName(PropertyInfo property, INamingContext namingContext) => _name;
	/// <inheritdoc />
	public string GetName(Type classType, INamingContext namingContext) => _name;
}