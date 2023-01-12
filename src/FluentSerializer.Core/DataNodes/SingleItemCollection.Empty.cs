using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace FluentSerializer.Core.DataNodes;

/// <inheritdoc cref="ISingleItemCollection{TItem}"/>
[StructLayout(LayoutKind.Sequential, Pack = 0)]
public readonly struct EmptySingleItemCollection<TItem> : ISingleItemCollection<TItem>
{
	/// <summary>
	/// Static instance for memory efficiency. <br />
	/// Please use <see cref="SingleItemCollection.Empty{TItem}"/>.
	/// </summary>
	public static readonly ISingleItemCollection<TItem> Value = new EmptySingleItemCollection<TItem>();

	/// <inheritdoc cref="ISingleItemCollection{TItem}.IsEmpty"/>
	public bool IsEmpty => true;

	/// <inheritdoc cref="ISingleItemCollection{TItem}.SingleItem"/>
	[MaybeNull]
	public TItem? SingleItem => default;

	/// <inheritdoc/>
	public TItem this[int index] => throw new IndexOutOfRangeException();

	/// <inheritdoc />
	public int Count => 0;

	/// <inheritdoc/>
	[ExcludeFromCodeCoverage(
#if NET5_0_OR_GREATER
	Justification = "The iterator is tested by the foreach test, and that should be enough"
#endif
	)]
	public IEnumerator<TItem> GetEnumerator() => EmptyItemIterator.Default;

	[ExcludeFromCodeCoverage(
#if NET5_0_OR_GREATER
	Justification = "The iterator is tested by the foreach test, and that should be enough"
#endif
	)]
	IEnumerator IEnumerable.GetEnumerator() => EmptyItemIterator.Default;


	[ExcludeFromCodeCoverage(
#if NET5_0_OR_GREATER
	Justification = "The iterator is tested by the foreach test, and that should be enough"
#endif
	)]
	private readonly struct EmptyItemIterator : IEnumerator<TItem>
	{
		internal static readonly EmptyItemIterator Default = default!;

		public TItem Current => throw new NotSupportedException();

		object IEnumerator.Current => throw new NotSupportedException();


		/// <inheritdoc/>
		public void Dispose()
		{
			// Method intentionally left empty.
		}

		/// <inheritdoc/>
		public bool MoveNext() => false;

		/// <inheritdoc/>
		public void Reset()
		{
			// Method intentionally left empty.
		}
	}
}
