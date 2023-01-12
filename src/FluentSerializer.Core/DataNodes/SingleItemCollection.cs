using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace FluentSerializer.Core.DataNodes;

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
public static class SingleItemCollection
{
	/// <summary>
	/// Reference the empty <see cref="ISingleItemCollection{TItem}"/>
	/// </summary>
	/// <typeparam name="TItem"></typeparam>
	public static ISingleItemCollection<TItem> Empty<TItem>() => EmptySingleItemCollection<TItem>.Value;

	/// <inheritdoc cref="ISingleItemCollection{TItem}"/>
	public static ISingleItemCollection<TItem> ForItem<TItem>(TItem item) => new SingleItemCollectionForItem<TItem>(item);
}
