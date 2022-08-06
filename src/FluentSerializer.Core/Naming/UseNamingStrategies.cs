using System.Runtime.CompilerServices;
using FluentSerializer.Core.Naming.NamingStrategies;

namespace FluentSerializer.Core.Naming;

internal readonly struct UseNamingStrategies : IUseNamingStrategies
{
	private static readonly CamelCaseNamingStrategy CamelCaseNamingStrategy = new();
	private static readonly NewCamelCaseNamingStrategy NewCamelCaseNamingStrategy = new();
	private static readonly LowerCaseNamingStrategy LowerCaseNamingStrategy = new();
	private static readonly PascalCaseNamingStrategy PascalCaseNamingStrategy = new();
	private static readonly NewPascalCaseNamingStrategy NewPascalCaseNamingStrategy = new();
	private static readonly SnakeCaseNamingStrategy SnakeCaseNamingStrategy = new();
	private static readonly NewSnakeCaseNamingStrategy NewSnakeCaseNamingStrategy = new();
	private static readonly KebabCaseNamingStrategy KebabCaseNamingStrategy = new();
	private static readonly NewKebabCaseNamingStrategy NewKebabCaseNamingStrategy = new();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public INamingStrategy CamelCase() => CamelCaseNamingStrategy;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public INewNamingStrategy CamelCaseNew() => NewCamelCaseNamingStrategy;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public INamingStrategy LowerCase() => LowerCaseNamingStrategy;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public INamingStrategy PascalCase() => PascalCaseNamingStrategy;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public INewNamingStrategy PascalCaseNew() => NewPascalCaseNamingStrategy;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public INamingStrategy SnakeCase() => SnakeCaseNamingStrategy;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public INewNamingStrategy SnakeCaseNew() => NewSnakeCaseNamingStrategy;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public INamingStrategy KebabCase() => KebabCaseNamingStrategy;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public INewNamingStrategy KebabCaseNew() => NewKebabCaseNamingStrategy;
}