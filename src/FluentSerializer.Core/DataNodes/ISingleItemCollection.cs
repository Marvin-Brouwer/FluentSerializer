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
