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
	public string GetName(in PropertyInfo property, in INamingContext _) => property.Name.Split('`')[0].ToLowerInvariant();
	/// <inheritdoc />
	public string GetName(in Type classType, in INamingContext _) => classType.Name.Split('`')[0].ToLowerInvariant();
}