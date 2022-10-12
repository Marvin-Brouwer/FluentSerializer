using System;
using System.Collections.Generic;

namespace FluentSerializer.Core.Converting;

/// <summary>
/// The Default comparer for <see cref="IConverter"/> instances. <br/>
/// These need a separate <see cref="IComparer{T}"/> like this because implementing <see cref="IComparable{T}"/> would require equality overloads everywhere.
/// </summary>
/// <remarks>
/// This comparer just misuses the fact that <see cref="IConverter"/> overrides the <see cref="IConverter.Equals(object?)"/> method
/// to check the <see cref="IConverter.ConverterHashCode"/>
/// </remarks>
public readonly struct ConverterComparer : IComparer<IConverter>
{
	/// <inheritdoc cref="ConverterComparer" />
	public static ConverterComparer Default { get; }

	/// <inheritdoc cref="IComparer{T}" />
	public int Compare(IConverter? x, IConverter? y)
	{
		if (x is null)
		{
			return y is null ? 0 : 1;
		}
		if (y is null)
		{
			return -1;
		}

		return x.Equals(y) ? 0 : 1;
	}
}
