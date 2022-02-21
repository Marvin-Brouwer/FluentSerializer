using System;
using System.Runtime.CompilerServices;
using Ardalis.GuardClauses;
using FluentSerializer.Core.Extensions;
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
	public static Func<INamingStrategy> Are(string name)
	{
		Guard.Against.InvalidName(name);

		return () => new CustomNamingStrategy(in name);
	}

	/// <inheritdoc cref="IUseNamingStrategies" />
	public static IUseNamingStrategies Use { get; } = new UseNamingStrategies();
}