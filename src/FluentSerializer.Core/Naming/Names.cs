using System;
using System.Runtime.CompilerServices;
using FluentSerializer.Core.Naming.NamingStrategies;

namespace FluentSerializer.Core.Naming;

public readonly struct Names
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Func<INamingStrategy> Are(string name) => () => new CustomNamingStrategy(name);
	public static IUseNamingStrategies Use { get; } = new UseNamingStrategies();
}