using System.Collections;
using System.Collections.Generic;

namespace FluentSerializer.Core.Comparing;

/// <summary>
/// Simple comparer between two references to any type of object or struct
/// </summary>
public readonly struct DefaultReferenceComparer : IEqualityComparer, IEqualityComparer<object>
{
	/// <summary>
	/// Static default implementation
	/// </summary>
	public static readonly DefaultReferenceComparer Default;

	/// <inheritdoc cref="IEqualityComparer"/>
	public new bool Equals(object? x, object? y)
	{
		if (x is null && y is null) return true;

		if (x is null) return false;
		if (y is null) return false;
		if (ReferenceEquals(x, y)) return true;

		return x.GetHashCode().Equals(y.GetHashCode());
	}

	/// <inheritdoc cref="IEqualityComparer"/>
	public int GetHashCode(object obj) => obj.GetHashCode();
}