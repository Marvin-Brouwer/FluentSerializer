using Ardalis.GuardClauses;

using FluentSerializer.Core.Extensions;
using FluentSerializer.Core.Naming.NamingStrategies;

using System;
using System.Runtime.CompilerServices;

namespace FluentSerializer.Core.Naming;

/// <summary>
/// Names configuration selector
/// </summary>
public readonly struct Names
{
	/// <summary>
	/// All names equal the explicit <paramref name="name"/> value for this mapping
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Func<INamingStrategy> Equal(string name)
	{
		Guard.Against.InvalidName(name);

		return () => new CustomNamingStrategy(in name);
	}

	/// <inheritdoc cref="IUseNamingStrategies" />
	public static IUseNamingStrategies Use { get; } = new UseNamingStrategies();
}