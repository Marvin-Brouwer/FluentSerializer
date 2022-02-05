using System;
using System.Reflection;
using FluentSerializer.Core.Context;

namespace FluentSerializer.Core.Naming.NamingStrategies;

/// <summary>
/// Convert class and property names to lowercase <br />
/// <example>
/// SomeName => somename
/// </example>
/// </summary>
public class LowerCaseNamingStrategy : INamingStrategy
{
	/// <inheritdoc />
	public string GetName(PropertyInfo property, INamingContext _) => property.Name.Split('`')[0].ToLowerInvariant();
	/// <inheritdoc />
	public string GetName(Type classType, INamingContext _) => classType.Name.Split('`')[0].ToLowerInvariant();
}