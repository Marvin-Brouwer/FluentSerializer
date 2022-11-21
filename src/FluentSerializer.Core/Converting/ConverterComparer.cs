using System.Collections;
using System.Collections.Generic;

namespace FluentSerializer.Core.Converting;

/// <summary>
/// The Default comparer for <see cref="IConverter"/> instances. <br/>
/// These need a separate <see cref="IComparer{T}"/> like this so the <see cref="IConverter.ConverterId"/> can be used to determine uniqueness
/// </summary>
public readonly struct ConverterComparer : IEqualityComparer<IConverter>
{
	/// <inheritdoc cref="ConverterComparer" />
	public static ConverterComparer Default { get; }

	/// <inheritdoc cref="IEqualityComparer"/>
	public bool Equals(IConverter? x, IConverter? y)
	{
		if (x is null && y is null) return true;

		if (x is null) return false;
		if (y is null) return false;
		if (ReferenceEquals(x, y)) return true;

		return x.ConverterId.Equals(y.ConverterId);
	}

	/// <inheritdoc cref="IEqualityComparer"/>
	public int GetHashCode(IConverter obj) => obj.ConverterId.GetHashCode();
}
