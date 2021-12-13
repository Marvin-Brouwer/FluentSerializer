using System.Runtime.CompilerServices;
using FluentSerializer.Core.Naming.NamingStrategies;

namespace FluentSerializer.Core.Naming;

internal readonly struct UseNamingStrategies : IUseNamingStrategies
{
	private static readonly CamelCaseNamingStrategy CamelCaseNamingStrategy = new();
	private static readonly LowerCaseNamingStrategy LowerCaseNamingStrategy = new();
	private static readonly PascalCaseNamingStrategy PascalCaseNamingStrategy = new();
	private static readonly SnakeCaseNamingStrategy SnakeCaseNamingStrategy = new();
	private static readonly KebabCaseNamingStrategy KebabCaseNamingStrategy = new();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public INamingStrategy CamelCase() => CamelCaseNamingStrategy;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public INamingStrategy LowerCase() => LowerCaseNamingStrategy;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public INamingStrategy PascalCase() => PascalCaseNamingStrategy;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public INamingStrategy SnakeCase() => SnakeCaseNamingStrategy;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public INamingStrategy KebabCase() => KebabCaseNamingStrategy;
}