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

#if NETSTANDARD2_0
		ConvertCasing(name.AsSpan(), ref characterSpan);
#else
		ConvertCasing(name, ref characterSpan);
#endif

		var newName = characterSpan.ToString();
		Guard.Against.InvalidName(newName);

#if NETSTANDARD2_0
		return newName.AsSpan();
#else
		return newName;
#endif
	}

	/// <summary>
	/// Convert the <paramref name="sourceSpan"/> and push into the <paramref name="characterSpan"/>.
	/// </summary>
	/// <param name="sourceSpan">
	/// The <see cref="ReadOnlySpan{T}"/> refering to the original input string.
	/// By default just pointing to <c>Type.Name</c> for classes and <c>PopertyInfo.Name</c> for properties.
	/// </param>
	/// <param name="characterSpan">
	/// The <see cref="Span{T}"/> of <see cref="char"/> for this class to insert the new names characters.
	/// </param>
	protected abstract void ConvertCasing(in ReadOnlySpan<char> sourceSpan, ref Span<char> characterSpan);
}