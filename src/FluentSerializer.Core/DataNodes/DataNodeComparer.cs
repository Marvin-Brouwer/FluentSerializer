using System;
using System.Collections.Generic;

namespace FluentSerializer.Core.DataNodes;

/// <summary>
/// Simple comparer between <see cref="IDataNode"/>s, relying on <see cref="GetHashCode"/>
/// </summary>
public readonly struct DataNodeComparer : IEqualityComparer<IEquatable<IDataNode>>
{
	/// <summary>
	/// Static default implementation
	/// </summary>
	public static readonly DataNodeComparer Default = new();


	/// <inheritdoc />
	public bool Equals(IEquatable<IDataNode>? x, IEquatable<IDataNode>? y)
	{
		if (x is null) return y is null;

		return x.GetHashCode().Equals(y?.GetHashCode());
	}

	/// <inheritdoc />
	public int GetHashCode(IEquatable<IDataNode>? obj) => obj?.GetHashCode() ?? 0;

	/// <summary>
	/// Get the combined HashCode of all objects passed to this method.
	/// This will call <see cref="GetHashCodeFor(IEnumerable{IEquatable{IDataNode}})"/> for collections
	/// and <see cref="GetHashCodeFor(in IEquatable{IDataNode}?)"/> for equatable objects.
	/// If null this will append 0 to the hashcode calculation
	/// If none of the above the objects <see cref="object.GetHashCode"/> will be added to the hashcode calculation.
	/// </summary>
	public int GetHashCodeForAll(params object?[] objects)
	{
		var hashCode = new HashCode();
		foreach (var obj in objects)
			hashCode.Add(GetHashCodeForObject(in obj));

		return hashCode.ToHashCode();
	}

	/// <inheritdoc cref="GetHashCodeForAll(object?[])"/>
	public int GetHashCodeForAll<TObj1>(in TObj1 obj1)
	{
		return GetHashCodeForObject(in obj1);
	}

	/// <inheritdoc cref="GetHashCodeForAll(object?[])"/>
	public int GetHashCodeForAll<TObj1, TObj2>(in TObj1 obj1, in TObj2 obj2)
	{
		var hashCode = new HashCode();

		hashCode.Add(GetHashCodeForObject(in obj1));
		hashCode.Add(GetHashCodeForObject(in obj2));

		return hashCode.ToHashCode();
	}

	/// <inheritdoc cref="GetHashCodeForAll(object?[])"/>
	public int GetHashCodeForAll<TObj1, TObj2, TObj3>(in TObj1 obj1, in TObj2 obj2, in TObj3 obj3)
	{
		var hashCode = new HashCode();

		hashCode.Add(GetHashCodeForObject(in obj1));
		hashCode.Add(GetHashCodeForObject(in obj2));
		hashCode.Add(GetHashCodeForObject(in obj3));

		return hashCode.ToHashCode();
	}

	private int GetHashCodeForObject<TObj>(in TObj? obj)
	{
		if (obj is null)
			return 0;
		if (obj is IEnumerable<IEquatable<IDataNode>> equatableCollection)
			return GetHashCodeFor(equatableCollection);
		if (obj is IEquatable<IDataNode> equatable)
			return GetHashCodeFor(equatable);
		

		return obj.GetHashCode();
	}

	/// <inheritdoc cref="IEqualityComparer{IDataNode}.GetHashCode(IDataNode)"/>
	private int GetHashCodeFor(in IEquatable<IDataNode>? equatable) => equatable is null ? 0 : GetHashCode(equatable);

	/// <summary>
	/// Returns a hashcode for every item in a collection
	/// </summary>
	/// <inheritdoc cref="IEqualityComparer{IDataNode}.GetHashCode(IDataNode)"/>
	private int GetHashCodeFor(IEnumerable<IEquatable<IDataNode>>? equatableCollection)
	{
		if (equatableCollection is null) return 0;

		var hash = new HashCode();
		foreach(var obj in equatableCollection) hash.Add(GetHashCode(obj));

		return hash.ToHashCode();
	}
}