using Ardalis.GuardClauses;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace FluentSerializer.Core.Mapping;

/// <inheritdoc cref="IScanList{TScanBy,TScanFor}"/>
public abstract class ScanList<TScanBy, TScanFor> : IScanList<TScanBy, TScanFor> 
	where TScanFor : class
{
	private readonly IReadOnlyList<TScanFor> _storedDataTypes;
#if (!DEBUG)
        private readonly Dictionary<TScanBy, TScanFor?> _cachedMappings = new();
#endif

	/// <inheritdoc />
	protected ScanList(in IReadOnlyList<TScanFor> dataTypes)
	{
		Guard.Against.Null(dataTypes, nameof(dataTypes));
		Guard.Against.InvalidInput(dataTypes, nameof(dataTypes), input => input.Any());

		_storedDataTypes = dataTypes;
	}

	/// <inheritdoc />
	public TScanFor? Scan(TScanBy key)
	{
		Guard.Against.Null(key, nameof(key));
#if (!DEBUG)
		if (_cachedMappings.ContainsKey(key)) return _cachedMappings[key];
#endif

		var matchingType = _storedDataTypes.FirstOrDefault(dataType => Compare(key, dataType));
#if (!DEBUG)
		_cachedMappings[key] = matchingType;
#endif

		return matchingType;
	}

	/// <inheritdoc />
	protected abstract bool Compare(TScanBy compareTo, in TScanFor dataType);

	#region IReadonlyList<T>

	/// <inheritdoc />
	public TScanFor this[int index] => _storedDataTypes[index];

	/// <inheritdoc />
	public int Count => _storedDataTypes.Count;

	/// <inheritdoc />
#if NET6_OR_GREATER
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
	public IEnumerator<TScanFor> GetEnumerator() => _storedDataTypes.GetEnumerator();
#if NET6_OR_GREATER
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
	IEnumerator IEnumerable.GetEnumerator() => _storedDataTypes.GetEnumerator();

	#endregion
}