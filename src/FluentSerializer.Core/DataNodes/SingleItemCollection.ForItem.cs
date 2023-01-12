using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace FluentSerializer.Core.DataNodes;

/// <inheritdoc cref="ISingleItemCollection{TItem}"/>
public sealed class SingleItemCollectionForItem<TItem> : ISingleItemCollection<TItem>
{
	private readonly SingleItemCollectionForItem<TItem>.SingleItemIterator _iterator;

	/// <inheritdoc cref="ISingleItemCollection{TItem}.IsEmpty"/>
	public bool IsEmpty => false;

	/// <inheritdoc cref="ISingleItemCollection{TItem}.SingleItem"/>
	[NotNull]
	public TItem? SingleItem { get; }

	internal bool Iterated { get; set; }

	/// <inheritdoc cref="IReadOnlyList{T}"/>
	public int Count => 1;

	/// <inheritdoc cref="IReadOnlyList{T}"/>
	public TItem this[int index] => index == 0
		? SingleItem!
		: throw new IndexOutOfRangeException();

	/// <inheritdoc cref="ISingleItemCollection{TItem}"/>
	internal SingleItemCollectionForItem(TItem item)
	{
		SingleItem = item;
		_iterator = new SingleItemIterator(this);
	}

	/// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
	[ExcludeFromCodeCoverage(
#if NET5_0_OR_GREATER
	Justification = "The iterator is tested by the foreach test, and that should be enough"
#endif
	)]
	public IEnumerator<TItem> GetEnumerator() => _iterator;

	[ExcludeFromCodeCoverage(
#if NET5_0_OR_GREATER
	Justification = "The iterator is tested by the foreach test, and that should be enough"
#endif
	)]
	IEnumerator IEnumerable.GetEnumerator() => _iterator;

	[ExcludeFromCodeCoverage(
#if NET5_0_OR_GREATER
	Justification = "The iterator is tested by the foreach test, and that should be enough"
#endif
	)]
	private readonly struct SingleItemIterator : IEnumerator<TItem>
	{
		private readonly SingleItemCollectionForItem<TItem> _collection;

		public SingleItemIterator(SingleItemCollectionForItem<TItem> collection)
		{
			_collection = collection;
		}

		public TItem Current => _collection.SingleItem!;

		object IEnumerator.Current => _collection.SingleItem!;

		public void Dispose()
		{
			// Method intentionally left empty.
		}

		public bool MoveNext()
		{
			if (_collection.Iterated) return false;
			_collection.Iterated = true;
			return true;
		}

		public void Reset()
		{
			_collection.Iterated = false;
		}
	}
}