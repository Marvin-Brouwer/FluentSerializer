using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;


namespace FluentSerializer.Core.DataNodes;

/// <summary>
/// A collection class specilized in <see cref="IEnumerable"/>s of a single <typeparamref name="TItem"/>. <br/>
/// These are especially useful for scenarios where a collection is necessary by interface, for example <see cref="IDataContainer{TValue}.Children"/>. <br/>
/// But they always will have only one child, for example JSON Properties
/// </summary>
/// <typeparam name="TItem"></typeparam>
public interface ISingleItemCollection<out TItem> : IReadOnlyList<TItem>
{
	/// <summary>
	/// Indicate whether there's actually an item in the collection.
	/// </summary>
#if NET5_0_OR_GREATER
	[MemberNotNullWhen(false, nameof(SingleItem))]
#endif
	bool IsEmpty { get; }

	/// <summary>
	/// Indicate whether there's actually an item in the collection.
	/// </summary>
	TItem? SingleItem { get; }
}

/// <summary>
/// A collection class specilized in <see cref="IEnumerable"/>s of a single Item. <br/>
/// These are especially useful for scenarios where a collection is necessary by interface, for example <see cref="IDataContainer{TValue}.Children"/>. <br/>
/// But they always will have only one child, for example JSON Properties
/// </summary>
[ExcludeFromCodeCoverage(
#if NET5_0_OR_GREATER
	Justification = "Array.Empty<T>() like passthrough"
#endif
)]
public readonly struct SingleItemCollection
{
	/// <summary>
	/// Reference the empty <see cref="ISingleItemCollection{TItem}"/>
	/// </summary>
	/// <typeparam name="TItem"></typeparam>
	public static ISingleItemCollection<TItem> Empty<TItem>() => EmptySingleItemCollection<TItem>.Value;
}

/// <inheritdoc cref="ISingleItemCollection{TItem}"/>
[ExcludeFromCodeCoverage(
#if NET5_0_OR_GREATER
	Justification = "TEMP for benchmark"
#endif
)]
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

/// <inheritdoc cref="ISingleItemCollection{TItem}"/>
[ExcludeFromCodeCoverage(
#if NET5_0_OR_GREATER
	Justification = "TEMP for benchmark"
#endif
)]
public sealed class SingleItemCollection<TItem> : ISingleItemCollection<TItem>
{
	private readonly SingleItemCollection<TItem>.SingleItemIterator _iterator;

	/// <inheritdoc cref="ISingleItemCollection{TItem}.IsEmpty"/>
	public bool IsEmpty => false;

	/// <inheritdoc cref="ISingleItemCollection{TItem}.SingleItem"/>
	public TItem? SingleItem { get; }

	internal bool Iterated { get; set; }

	/// <inheritdoc cref="IReadOnlyList{T}"/>
	public int Count => 1;

	/// <inheritdoc cref="IReadOnlyList{T}"/>
	public TItem this[int index] => index == 0
		? SingleItem ?? throw new IndexOutOfRangeException()
		: throw new IndexOutOfRangeException() ;

	/// <inheritdoc cref="ISingleItemCollection{TItem}"/>
	public SingleItemCollection(TItem  item)
	{
		SingleItem = item;
		_iterator = new SingleItemIterator(this);
	}

	/// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
	public IEnumerator<TItem> GetEnumerator() => _iterator;
	IEnumerator IEnumerable.GetEnumerator() => _iterator;

	private readonly struct SingleItemIterator : IEnumerator<TItem>
	{
		private readonly SingleItemCollection<TItem> _collection;

		public SingleItemIterator(SingleItemCollection<TItem> collection)
		{
			_collection = collection;
		}

		public TItem Current => _collection.SingleItem!;

		object IEnumerator.Current => _collection.SingleItem!;

		public void Dispose()
		{
			throw new NotImplementedException();
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