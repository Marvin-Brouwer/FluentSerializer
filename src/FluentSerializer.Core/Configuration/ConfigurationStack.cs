using System.Collections;
using System.Collections.Generic;

namespace FluentSerializer.Core.Configuration;

/// <inheritdoc cref="IConfigurationStack{T}" />
public sealed class ConfigurationStack<T> : SortedSet<T>, IConfigurationStack<T>
{

	/// <inheritdoc />
	public IConfigurationStack<T> Use(T item, bool forceTop = false)
	{
		if (forceTop && Contains(item)) Remove(item);

		Add(item);
		return this;
	}

	/// <inheritdoc />
	public new IEnumerator<T> GetEnumerator()
	{
		return Reverse().GetEnumerator();
	}

	/// <inheritdoc />
	IEnumerator IEnumerable.GetEnumerator()
	{
		return Reverse().GetEnumerator();
	}
}
