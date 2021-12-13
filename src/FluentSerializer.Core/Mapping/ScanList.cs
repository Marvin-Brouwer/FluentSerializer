using Ardalis.GuardClauses;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FluentSerializer.Core.Mapping;

/// <inheritdoc cref="IScanList{TScanBy,TScanFor}"/>
public abstract class ScanList<TScanBy, TScanFor> : IScanList<TScanBy, TScanFor> 
	where TScanFor : class
{
	private readonly IReadOnlyList<TScanFor> _storedDataTypes;
#if (!DEBUG)
	private readonly Dictionary<TScanBy, TScanFor?> _cachedMappings = new();
#endif

	protected ScanList(IReadOnlyList<TScanFor> dataTypes)
	{
		Guard.Against.Null(dataTypes, nameof(dataTypes));
		Guard.Against.InvalidInput(dataTypes, nameof(dataTypes), input => input.Any());

		_storedDataTypes = dataTypes;
	}

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

	protected abstract bool Compare(TScanBy compareTo, TScanFor dataType);

	#region IReadonlyList<T>

	public TScanFor this[int index] => _storedDataTypes[index];

	public int Count => _storedDataTypes.Count;

	public IEnumerator<TScanFor> GetEnumerator() => _storedDataTypes.GetEnumerator();
	IEnumerator IEnumerable.GetEnumerator() => _storedDataTypes.GetEnumerator();

	#endregion
}