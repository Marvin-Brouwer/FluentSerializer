using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace FluentSerializer.Core.DataNodes;

/// <summary>
/// Simple comparer between <see cref="IDataNode"/>s, relying on <see cref="GetHashCode"/>
/// </summary>
/// <remarks>
/// <b>NOTE: </b>This <see cref="IEqualityComparer{T}"/> heavily relies on the <see cref="IDataNode"/>
/// to have a custom implementation <br />
/// of the <see cref="object.GetHashCode"/> delegating to the <br />
/// <see cref="Default"/>.<see cref="GetHashCodeForObject{TObj}"/> or <br />
/// <see cref="Default"/>.<see cref="GetHashCodeForAll"/> method
/// </remarks>
public readonly struct DataNodeComparer : IEqualityComparer<IDataNode>
{
	/// <summary>
	/// Static default implementation
	/// </summary>
	public static readonly DataNodeComparer Default;

	/// <inheritdoc />
	[ExcludeFromCodeCoverage, Obsolete(error: true, message: $"Please create your own implementation of {nameof(Equals)}")]
	public bool Equals(IDataNode? x, IDataNode? y) => throw new NotSupportedException($"Please create your own implementation of {nameof(Equals)}");

	/// <inheritdoc />
	[ExcludeFromCodeCoverage, Obsolete(error: true, message: $"Please create your own implementation of {nameof(GetHashCode)}")]
	public int GetHashCode(IDataNode? obj) => throw new NotSupportedException($"Please create your own implementation of {nameof(GetHashCode)}");

	/// <summary>
	/// Get the combined HashCode of all objects passed to this method.
	/// This will call <see cref="GetHashCodeFor(in IEnumerable{IDataNode})"/> for collections
	/// and <see cref="GetHashCodeFor(in IDataNode?)"/> for equatable objects.
	/// If null this will append 0 to the hashcode calculation
	/// If none of the above the objects <see cref="object.GetHashCode"/> will be added to the hashcode calculation.
	/// </summary>
#if NET6_0_OR_GREATER
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
	public HashCode GetHashCodeForAll(params object?[] objects)
	{
		var hashCode = new HashCode();
		foreach (var obj in objects)
			hashCode.Add(GetHashCodeForObject(in obj).ToHashCode());

		return hashCode;
	}

	/// <inheritdoc cref="GetHashCodeForAll(object?[])"/>
#if NET6_0_OR_GREATER
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
	public HashCode GetHashCodeForAll<TObj1>(in TObj1 obj1)
	{
		return GetHashCodeForObject(in obj1);
	}

	/// <inheritdoc cref="GetHashCodeForAll(object?[])"/>
#if NET6_0_OR_GREATER
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
	public HashCode GetHashCodeForAll<TObj1, TObj2>(in TObj1 obj1, in TObj2 obj2)
	{
		var hashCode = new HashCode();

		hashCode.Add(GetHashCodeForObject(in obj1).ToHashCode());
		hashCode.Add(GetHashCodeForObject(in obj2).ToHashCode());

		return hashCode;
	}

	/// <inheritdoc cref="GetHashCodeForAll(object?[])"/>
#if NET6_0_OR_GREATER
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
	public HashCode GetHashCodeForAll<TObj1, TObj2, TObj3>(in TObj1 obj1, in TObj2 obj2, in TObj3 obj3)
	{
		var hashCode = new HashCode();

		hashCode.Add(GetHashCodeForObject(in obj1).ToHashCode());
		hashCode.Add(GetHashCodeForObject(in obj2).ToHashCode());
		hashCode.Add(GetHashCodeForObject(in obj3).ToHashCode());

		return hashCode;
	}

#if NET6_0_OR_GREATER
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
	private HashCode GetHashCodeForObject<TObj>(in TObj? obj)
	{
		if (obj is null)
			return default;
		if (obj is IDataValue value && value.Value is null)
			return default;
		if (obj is string stringValue && string.IsNullOrEmpty(stringValue))
			return default;
		if (obj is IEnumerable<IDataNode> equatableCollection)
			return GetHashCodeFor(in equatableCollection);
		if (obj is IDataNode equatable)
			return GetHashCodeFor(in equatable);

		var hashCode = new HashCode();
		hashCode.Add(obj.GetHashCode());
		return hashCode;
	}

#if NET6_0_OR_GREATER
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
	private static HashCode GetHashCodeFor(in IDataNode? node)
	{
		if (node is null) return default;

		var hash = new HashCode();
		hash.Add(node.GetHashCode());
		return hash;
	}

#if NET6_0_OR_GREATER
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
	private HashCode GetHashCodeFor(in IEnumerable<IDataNode>? nodeCollection)
	{
		if (nodeCollection is null) return default;

		var hash = new HashCode();
		foreach (var obj in nodeCollection) hash.Add(GetHashCode(obj));

		return hash;
	}
}