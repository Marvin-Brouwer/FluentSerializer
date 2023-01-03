using System;
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
	/// This will replace an existing item at the current position, use <paramref name="forceTop"/> = <c>true</c> to re-add it to the top
	/// </remarks>
	public IConfigurationStack<T> Use(T item, bool forceTop = false);

	/// <summary>
	/// Insert this <typeparamref name="T"/> at the top of this configuration,
	/// the <see cref="Func{TResult}"/> will be invoked immediately if called like this.
	/// </summary>
	/// <remarks>
	/// This will replace an existing item at the current position, use <paramref name="forceTop"/> = <c>true</c> to re-add it to the top
	/// </remarks>
	public IConfigurationStack<T> Use(Func<T> item, bool forceTop = false);
}
