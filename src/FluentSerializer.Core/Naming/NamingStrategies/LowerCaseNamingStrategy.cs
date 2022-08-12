using FluentSerializer.Core.Constants;
using FluentSerializer.Core.Context;

using System;
using System.Reflection;

namespace FluentSerializer.Core.Naming.NamingStrategies;

/// <summary>
/// Convert class and property names to lowercase <br />
/// <example>
/// SomeName => somename
/// </example>
/// </summary>
public readonly struct LowerCaseNamingStrategy : INamingStrategy
{
	/// <inheritdoc />
	public ReadOnlySpan<char> GetName(in PropertyInfo property, in Type propertyType, in INamingContext _) => GetName(property.Name);

	/// <inheritdoc />
	public ReadOnlySpan<char> GetName(in Type classType, in INamingContext _) => GetName(classType.Name);

	private ReadOnlySpan<char> GetName(in string name)
	{
		var genericIndex = name.IndexOf(NamingConstants.GenericTypeMarker);
		if (genericIndex == -1) return name.ToLowerInvariant();

		Span<char> nameSpan = stackalloc char[genericIndex];
		name.AsSpan()[..genericIndex].ToLowerInvariant(nameSpan);

		return nameSpan.ToString();
	}
}