using System;
using System.Globalization;
using System.Reflection;
using Ardalis.GuardClauses;
using FluentSerializer.Core.Constants;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Extensions;

namespace FluentSerializer.Core.Naming.NamingStrategies;

/// <summary>
/// Base clas for convertin class and property names
/// </summary>
public abstract class AbstractSpanNamingStrategy : INewNamingStrategy
{
	/// <summary>
	/// Current character count, will be reset in the default <see cref="GetName(in string)"/> method.
	/// </summary>
	protected int CharCount = 0;

	/// <inheritdoc />
	public virtual ReadOnlySpan<char> GetName(in PropertyInfo property, in Type propertyType, in INamingContext namingContext) => GetName(property.Name);
	/// <inheritdoc />
	public virtual ReadOnlySpan<char> GetName(in Type classType, in INamingContext namingContext) => GetName(classType.Name);

	/// <summary>
	/// Convert a string value to camelCase
	/// </summary>
	protected virtual ReadOnlySpan<char> GetName(in string name)
	{
		Span<char> characterSpan = stackalloc char[name.Length];

		ConvertCasing(name, ref characterSpan);
		CharCount = 0;

		var newName = characterSpan.ToString();
		Guard.Against.InvalidName(newName);
		return newName;
	}

	/// <remarks>
	/// Convert the <paramref name="sourceSpan"/> and push into the <paramref name="characterSpan"/>.
	/// </remarks>
	/// <param name="sourceSpan">
	/// The <see cref="ReadOnlySpan{T}"/> refering to the original input string.
	/// By default just pointing to <c>Type.Name</c> for classes and <c>PopertyInfo.Name</c> for properties.
	/// </param>
	/// <param name="characterSpan">
	/// The <see cref="Span{T}"/> of <see cref="char"/> for this class to insert the new names characters.
	/// </param>
	protected abstract void ConvertCasing(in ReadOnlySpan<char> sourceSpan, ref Span<char> characterSpan);
}