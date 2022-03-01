using System.Collections.Generic;

namespace FluentSerializer.Core.Configuration;

/// <summary>
/// Collection containing Configuration items with a specific order and only allowing unique items
/// </summary>
public interface IConfigurationStack<T> : IEnumerable<T>
{
	/// <summary>
	/// Insert this <typeparamref name="T"/> at the top of this configuration
	/// </summary>
	/// <remarks>
	/// If a similar item with the same <see cref="object.GetHashCode()"/> result already exists
	/// this will replace that item at that position, use <paramref name="forceTop"/> = <c>true</c> to re-add it to the top
	/// </remarks>
	public IConfigurationStack<T> Use(T item, bool forceTop = false);
}
