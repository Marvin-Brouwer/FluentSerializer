using System;
using System.Collections.Generic;

namespace FluentSerializer.Core.DataNodes;

/// <summary>
/// Simple comparer between <see cref="IDataNode"/>s, relying on <see cref="IEquatable{IDataNode}.GetHashCode()"/>
/// </summary>
public readonly struct DataNodeComparer : IEqualityComparer<IEquatable<IDataNode>>
{
	public static readonly DataNodeComparer Default = new();

	public bool Equals(IEquatable<IDataNode>? x, IEquatable<IDataNode>? y)
	{
		if (x is null) return y is null;

		return x.GetHashCode().Equals(y?.GetHashCode());
	}

	public int GetHashCode(IEquatable<IDataNode>? obj) => obj?.GetHashCode() ?? 0;

	/// <summary>
	/// Get the combined HashCode of all objects passed to this method.
	/// This will call <see cref="GetHashCodeFor(IEnumerable{IEquatable{IDataNode}})"/> for collections
	/// and <see cref="GetHashCodeFor(IEquatable{IDataNode}?)"/> for equatable objects.
	/// If null this will append 0 to the hascode calculation
	/// If none of the above the objects <see cref="object.GetHashCode"/> will be added to the hashcode calculation.
	/// </summary>
	public int GetHashCodeForAll(params object?[] objects)
	{
		var hashCode = new HashCode();
		foreach(var obj in objects)
		{
			if (obj is null)
			{
				hashCode.Add(0);
				continue;
			}
			if (obj is IEnumerable<IEquatable<IDataNode>> equatableCollection)
			{
				hashCode.Add(GetHashCodeFor(equatableCollection));
				continue;
			}
			if (obj is IEquatable<IDataNode> equatable)
			{
				hashCode.Add(GetHashCodeFor(equatable));
				continue;
			}

			hashCode.Add(obj.GetHashCode());
		}

		return hashCode.ToHashCode();
	}

	/// <inheritdoc cref="IEqualityComparer{IDataNode}.GetHashCode(IDataNode)"/>
	public int GetHashCodeFor(IEquatable<IDataNode>? equatable) => equatable is null ? 0 : GetHashCode(equatable);

	/// <summary>
	/// Returns a hashcode for every item in a collection
	/// </summary>
	/// <inheritdoc cref="IEqualityComparer{IDataNode}.GetHashCode(IDataNode)"/>
	public int GetHashCodeFor(IEnumerable<IEquatable<IDataNode>> equatableCollection)
	{
		if (equatableCollection is null) return 0;

		var hash = new HashCode();
		foreach(var obj in equatableCollection) hash.Add(GetHashCode(obj));

		return hash.ToHashCode();
	}
}