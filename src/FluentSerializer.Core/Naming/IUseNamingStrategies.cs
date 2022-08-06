
using FluentSerializer.Core.Naming.NamingStrategies;

namespace FluentSerializer.Core.Naming;

/// <summary>
/// Use an <see cref="INamingStrategy"/> for this mapping
/// </summary>
/// <remarks>
/// For using a custom <see cref="INamingStrategy"/> create an extension method on <see cref="IUseNamingStrategies"/>
/// </remarks>
public interface IUseNamingStrategies
{
	/// <inheritdoc cref="CamelCaseNamingStrategy" />
	INamingStrategy CamelCase();
	/// <inheritdoc cref="CamelCaseNamingStrategy" />
	INewNamingStrategy CamelCaseNew();
	/// <inheritdoc cref="LowerCaseNamingStrategy" />
	INamingStrategy LowerCase();
	/// <inheritdoc cref="PascalCaseNamingStrategy" />
	INamingStrategy PascalCase();
	/// <inheritdoc cref="PascalCaseNamingStrategy" />
	INewNamingStrategy PascalCaseNew();
	/// <inheritdoc cref="SnakeCaseNamingStrategy" />
	INamingStrategy SnakeCase();
	/// <inheritdoc cref="SnakeCaseNamingStrategy" />
	INewNamingStrategy SnakeCaseNew();
	/// <inheritdoc cref="KebabCaseNamingStrategy" />
	INamingStrategy KebabCase();
	/// <inheritdoc cref="KebabCaseNamingStrategy" />
	INewNamingStrategy KebabCaseNew();
}