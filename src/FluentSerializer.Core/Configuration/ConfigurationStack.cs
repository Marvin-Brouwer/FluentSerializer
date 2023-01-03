using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FluentSerializer.Core.Configuration;

/// <inheritdoc cref="IConfigurationStack{T}" />
public sealed class ConfigurationStack<T> : IConfigurationStack<T>
{
	private readonly IEqualityComparer<T> _valueComparer;
	private readonly List<T> _innerList;

	/// <inheritdoc cref="IConfigurationStack{T}" />
	public ConfigurationStack(IEqualityComparer<T> valueComparer, params T[] initialValues)
	{
		_valueComparer = valueComparer;
		_innerList = new List<T>(initialValues.Reverse());
	}

	/// <inheritdoc />
	public IConfigurationStack<T> Use(T item, bool forceTop = false)
	{
		var existingIndex = _innerList.FindIndex(listItem => _valueComparer.Equals(item, listItem));
		if (existingIndex == -1)
		{
			_innerList.Insert(0, item);
			return this;
		}
		if (forceTop)
		{
			_innerList.RemoveAt(existingIndex);
			_innerList.Insert(0, item);
			return this;
		}

		_innerList[existingIndex] = item;

		return this;
	}

	/// <inheritdoc />
	public IConfigurationStack<T> Use(Func<T> item, bool forceTop = false) => Use(item(), forceTop);

	/// <inheritdoc />
	public IEnumerator<T> GetEnumerator()
	{
		return _innerList.GetEnumerator();
	}

	/// <inheritdoc />
	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
