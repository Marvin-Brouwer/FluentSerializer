using System;
using System.Runtime.CompilerServices;
using FluentSerializer.Core.Naming.NamingStrategies;

namespace FluentSerializer.Core.Naming;

/// <summary>
/// Names configuration selector
/// </summary>
public readonly struct Names
{
	/// <summary>
	/// All names are the explicit <paramref name="name"/> value for this mapping
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Func<INamingStrategy> Are(string name) => () => new CustomNamingStrategy(in name);

	/// <inheritdoc cref="IUseNamingStrategies" />
	public static IUseNamingStrategies Use { get; } = new UseNamingStrategies();
}