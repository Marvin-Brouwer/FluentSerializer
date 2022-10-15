using System.Collections;

namespace FluentSerializer.Core.Comparing;

/// <summary>
/// Simple comparer between two references
/// </summary>
public readonly struct DefaultReferenceComparer : IEqualityComparer
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