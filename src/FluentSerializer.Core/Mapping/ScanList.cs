using Ardalis.GuardClauses;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace FluentSerializer.Core.Mapping;

/// <inheritdoc cref="IScanList{TScanBy,TScanFor}" />
public abstract class ScanList<TScanBy, TScanFor> : IScanList<TScanBy, TScanFor> 
	where TScanBy : notnull
	where TScanFor : class
{
	private readonly IReadOnlyCollection<TScanFor> _storedDataTypes;
#if (!DEBUG)
        private readonly Dictionary<TScanBy, TScanFor?> _cachedMappings = new();
#endif

	/// <inheritdoc  cref="IScanList{TScanBy,TScanFor}" />
	protected ScanList(in IReadOnlyCollection<TScanFor> dataTypes)
	{
		Guard.Against.Null(dataTypes, nameof(dataTypes));

		_storedDataTypes = dataTypes;
	}

	/// <inheritdoc />
	public TScanFor? Scan(TScanBy key)
	{
		Guard.Against.Null(key, nameof(key));
		if (_storedDataTypes.Count == 0) return null;
#if (!DEBUG)
		if (_cachedMappings.ContainsKey(key)) return _cachedMappings[key];
#endif

		var matchingType = (TScanFor?)null;
		foreach(var dataType in _storedDataTypes)
		{
			if (!Compare(key, dataType)) continue;
			matchingType = dataType;
		}

#if (!DEBUG)
		_cachedMappings[key] = matchingType;
#endif

		return matchingType;
	}

	/// <summary>
	/// Implement the selector logic here
	/// </summary>
	protected abstract bool Compare(TScanBy compareTo, in TScanFor dataType);

	#region IReadOnlyCollection<T>

	/// <inheritdoc />
	public int Count => _storedDataTypes.Count;

	/// <inheritdoc />
#if NET6_0_OR_GREATER
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
	public IEnumerator<TScanFor> GetEnumerator() => _storedDataTypes.GetEnumerator();
#if NET6_0_OR_GREATER
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
	IEnumerator IEnumerable.GetEnumerator() => _storedDataTypes.GetEnumerator();

	#endregion
}