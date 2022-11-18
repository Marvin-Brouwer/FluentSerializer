using Ardalis.GuardClauses;

using FluentSerializer.Core.Context;
using FluentSerializer.Core.Extensions;

using System;
using System.Reflection;

namespace FluentSerializer.Core.Naming.NamingStrategies;

/// <summary>
/// Base class for converting class and property names
/// </summary>
public abstract class AbstractSpanNamingStrategy : INamingStrategy
{
	/// <inheritdoc />
	public virtual ReadOnlySpan<char> GetName(in PropertyInfo propertyInfo, in Type propertyType, in INamingContext namingContext) => GetName(propertyInfo.Name);
	/// <inheritdoc />
	public virtual ReadOnlySpan<char> GetName(in Type classType, in INamingContext namingContext) => GetName(classType.Name);

	/// <summary>
	/// Convert a string value to camelCase
	/// </summary>
	protected virtual ReadOnlySpan<char> GetName(string name)
	{
		Span<char> characterSpan = stackalloc char[name.Length];

		ConvertCasing(name, ref characterSpan);

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