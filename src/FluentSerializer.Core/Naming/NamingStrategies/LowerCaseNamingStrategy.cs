using FluentSerializer.Core.Constants;
using FluentSerializer.Core.Context;

using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace FluentSerializer.Core.Naming.NamingStrategies;

/// <summary>
/// Convert class and property names to lowercase <br />
/// <example>
/// SomeName => somename
/// </example>
/// </summary>
[StructLayout(LayoutKind.Explicit, Pack = 0, Size = 1)]
public readonly struct LowerCaseNamingStrategy : INamingStrategy
{
	/// <inheritdoc />
	public ReadOnlySpan<char> GetName(in PropertyInfo propertyInfo, in Type propertyType, in INamingContext namingContext) => GetName(propertyInfo.Name);

	/// <inheritdoc />
	public ReadOnlySpan<char> GetName(in Type classType, in INamingContext namingContext) => GetName(classType.Name);

	private ReadOnlySpan<char> GetName(string name)
	{
		var genericIndex = name.IndexOf(NamingConstants.GenericTypeMarker);
		if (genericIndex == -1) return name.ToLowerInvariant();

		Span<char> nameSpan = stackalloc char[genericIndex];
		name.AsSpan()[..genericIndex].ToLowerInvariant(nameSpan);

		return nameSpan.ToString();
	}
}