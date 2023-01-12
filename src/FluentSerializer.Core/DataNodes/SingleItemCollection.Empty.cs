using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace FluentSerializer.Core.DataNodes;

/// <inheritdoc cref="ISingleItemCollection{TItem}"/>
[StructLayout(LayoutKind.Sequential, Pack = 0)]
internal readonly struct EmptySingleItemCollection<TItem> : ISingleItemCollection<TItem>, IEnumerator<TItem>
{
	public static readonly ISingleItemCollection<TItem> Value = new EmptySingleItemCollection<TItem>();

	/// <inheritdoc cref="ISingleItemCollection{TItem}.IsEmpty"/>
	public bool IsEmpty => true;

	/// <inheritdoc cref="ISingleItemCollection{TItem}.SingleItem"/>
	public TItem? SingleItem => default;

	public TItem this[int index] => Array.Empty<TItem>()[index];

	public int Count => 0;

	public TItem Current => throw new NotSupportedException();

	object IEnumerator.Current => throw new NotSupportedException();

	public IEnumerator<TItem> GetEnumerator() => this;
	IEnumerator IEnumerable.GetEnumerator() => this;

	public void Dispose()
	{
		// Method intentionally left empty.
	}

	public bool MoveNext() => false;

	public void Reset()
	{
		// Method intentionally left empty.
	}
}
