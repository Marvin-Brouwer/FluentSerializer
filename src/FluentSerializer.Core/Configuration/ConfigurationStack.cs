using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FluentSerializer.Core.Configuration;

/// <inheritdoc cref="IConfigurationStack{T}" />
[Serializable]
public sealed class ConfigurationStack<T> : SortedSet<T>, IConfigurationStack<T>
{
	/// <inheritdoc cref="IConfigurationStack{T}" />
	public ConfigurationStack(IComparer<T> comparer) : base(comparer) { }

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

	/// <inheritdoc cref="IConfigurationStack{T}" />
	private ConfigurationStack(SerializationInfo info, StreamingContext context) : base(info, context) { }

	/// <inheritdoc cref="ISerializable" />
	protected override void GetObjectData(SerializationInfo info, StreamingContext context) => base.GetObjectData(info, context);
}
