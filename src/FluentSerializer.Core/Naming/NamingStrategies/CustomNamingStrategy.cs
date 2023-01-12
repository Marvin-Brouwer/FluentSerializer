using FluentSerializer.Core.Context;

using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace FluentSerializer.Core.Naming.NamingStrategies;

/// <summary>
/// Use a preconfigured value
/// </summary>
[StructLayout(LayoutKind.Explicit, Pack = 0)]
public readonly struct CustomNamingStrategy : INamingStrategy
{
	[FieldOffset(0)]
	private readonly string _name;

	/// <summary>
	/// Use <paramref name="name"/> as a preconfigured value for this strategy
	/// </summary>
	public CustomNamingStrategy(in string name)
	{
		_name = name;
	}

	/// <inheritdoc />
	public ReadOnlySpan<char> GetName(in PropertyInfo propertyInfo, in Type propertyType, in INamingContext namingContext) => _name
#if NETSTANDARD2_0
		.AsSpan()
#endif
	;
	/// <inheritdoc />
	public ReadOnlySpan<char> GetName(in Type classType, in INamingContext namingContext) => _name
#if NETSTANDARD2_0
		.AsSpan()
#endif
	;
}